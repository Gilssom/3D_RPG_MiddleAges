using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForComplete, // >> �ڵ� �Ϸ� ����Ʈ�� �ƴ� ���� �ϷḦ ���Ѿ� �Ǵ� ������ ����Ʈ
}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region #Event
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceldHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion

    [SerializeField]
    private Category m_Category;
    [SerializeField]
    private Sprite m_Icon;

    [Header("����Ʈ ���� �ۼ�")]
    [SerializeField]
    private string m_CodeName;
    [SerializeField]
    private string m_DisPlayName;
    [SerializeField, TextArea]
    private string m_Description;

    // 1. ���� ��� ���� �������� 10���� ��ƶ�
    // 2. ���� �������� 10���� ��ƶ�
    // 2. ��� �������� 10���� ��ƶ� -> ����� 5���� ��ƶ�
    // �������� Task �� �� �ִ� ��Ȳ�� ���� �� �ִ�. -> Task Group �� �ʿ�
    [Header("����Ʈ�� ����")]
    [SerializeField]
    private TaskGroup[] m_TaskGroup;

    [Header("����Ʈ ����")]
    [SerializeField]
    private Reward[] m_Rewards;

    [Header("����Ʈ �ɼ�")]
    [SerializeField]
    private bool m_UseAutoComplete; // �ڵ� �Ϸ� ����Ʈ����
    [SerializeField]
    private bool m_IsCancelable; // ����� �� ���� ����Ʈ����
    // ���̺긦 ��ų ����Ʈ���� =
    //          Ʃ�丮���� ���̵� ����Ʈ�� ����ϰų� ���� ���� ������ �Ǿ�� �� �ʿ䰡 ���� ����
    [SerializeField]
    private bool m_IsSavable; 

    [Header("����Ʈ ���� �� ��� ����")]
    [SerializeField]
    private Condition[] m_AcceptionConditions;
    [SerializeField]
    private Condition[] m_CancelConditions;

    private int m_CurrentTaskGroupIndex;

    // p_ :: Property
    public Category p_Category => m_Category;
    public Sprite p_Icon => m_Icon;
    public string p_CodeName => m_CodeName;
    public string p_DisPlayName => m_DisPlayName;
    public string p_Description => m_Description;

    public QuestState m_State { get; private set; }
    public TaskGroup p_CurrentTaskGroup => m_TaskGroup[m_CurrentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> p_TaskGroups => m_TaskGroup;
    public IReadOnlyList<Reward> p_Rewards => m_Rewards;

    public bool p_IsRegistered => m_State != QuestState.Inactive;
    public bool p_IsCompletable => m_State == QuestState.WaitingForComplete;
    public bool p_IsComplete => m_State == QuestState.Complete;
    public bool p_IsCancle => m_State == QuestState.Cancel;
    public virtual bool p_IsCancelable => m_IsCancelable && m_CancelConditions.All(x => x.IsPass(this));
    public bool p_IsAcceptable => m_AcceptionConditions.All(x => x.IsPass(this));
    public virtual bool p_IsSavable => m_IsSavable;

    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event CompletedHandler onCompleted;
    public event CanceldHandler onCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;

    // Awake ��Ȱ�� �Լ� > Quest �� System �� ��ϵǾ��� �� ����
    public void OnRegister()
    {
        // Assert :: ���ڷ� ���� ���� false�� ���� ������ Error �� ����ش�.
        // ���� �Ͼ���� �ȵǴ� ������ �Ͼ�� �� �����ϱ� ���� �ڵ�
        // Debugging �ڵ�ν� ������ Build �ؼ� �̾Ƴ��� Assert Code �� ���ð� �ȴ�. > ���ɿ� ���� X > ����� ���α׷���
        Debug.Assert(!p_IsRegistered, "�� ����Ʈ�� �̹� ��ϵ� ����Ʈ�Դϴ�.");

        foreach (var taskGroup in m_TaskGroup)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.p_Tasks)
                task.onSuccessChanged += OnSuccessChanged; // CallBack
        }

        m_State = QuestState.Running;
        p_CurrentTaskGroup.Start();
    }

    // ���� �޴� �Լ�
    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(p_IsRegistered, "�� ����Ʈ�� �̹� ��ϵ� ����Ʈ�Դϴ�.");
        Debug.Assert(!p_IsCancle, "�� ����Ʈ�� �̹� ��ҵ� ����Ʈ�Դϴ�.");

        if (p_IsComplete)
            return; // ����Ʈ�� �Ϸᰡ �Ǿ�� ���� ���� �� �ִ� ��Ȳ�� ���� �� �ֱ� ����

        p_CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (p_CurrentTaskGroup.p_IsAllTaskComplete)
        {
            if (m_CurrentTaskGroupIndex + 1 == m_TaskGroup.Length)
            {
                m_State = QuestState.WaitingForComplete;
                if (m_UseAutoComplete)
                    Complete();
            }
            else // ���� TaskGroup �� �����Ѵٸ�
            {
                var prevTaskGroup = m_TaskGroup[m_CurrentTaskGroupIndex++];
                prevTaskGroup.End();
                p_CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, p_CurrentTaskGroup, prevTaskGroup);
            }
        }
        else
            m_State = QuestState.Running;
    }

    // ����Ʈ�� �Ϸ��ϴ� �Լ�
    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in m_TaskGroup)
            taskGroup.Complete();

        m_State = QuestState.Complete;

        foreach (var reward in m_Rewards)
            reward.Give(this);

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;
    }

    // ����Ʈ�� ����ϴ� �Լ�
    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(p_IsCancelable, "�� ����Ʈ�� ������ �� ���� ����Ʈ�Դϴ�.");

        m_State = QuestState.Cancel;
        onCanceled?.Invoke(this);
    }

    // Quest System �� �Ѱ��� Quest ���纻 :: Cloning
    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.m_TaskGroup = m_TaskGroup.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }
    #region # Json Save & Load
    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            m_CodeName = m_CodeName,
            m_State = m_State,
            m_TaskGroupIndex = m_CurrentTaskGroupIndex,
            m_TaskSeccessCounts = p_CurrentTaskGroup.p_Tasks.Select(x => x.p_CurrentSucceses).ToArray()
        };
    }

    public void LoadFrom(QuestSaveData saveData)
    {
        m_State = saveData.m_State;
        m_CurrentTaskGroupIndex = saveData.m_TaskGroupIndex;

        for (int i = 0; i < m_CurrentTaskGroupIndex; i++)
        {
            var taskGroup = m_TaskGroup[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        for (int i = 0; i < saveData.m_TaskSeccessCounts.Length; i++)
        {
            p_CurrentTaskGroup.Start();
            p_CurrentTaskGroup.p_Tasks[i].p_CurrentSucceses = saveData.m_TaskSeccessCounts[i];
        }
    }
    #endregion

    private void OnSuccessChanged(Task task, int currentSuccess, int prevSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);

    // Conditional Attribute �� ���ڷ� ������ Simbol ���� ����Ǿ� ������
    // �Լ��� �����ϰ� �ƴ϶�� �Լ��� �����ϰ� ���� >> using System.Diagnostics; ����
    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(p_IsRegistered, "�� ����Ʈ�� �̹� ��ϵ� ����Ʈ�Դϴ�.");
        Debug.Assert(!p_IsCancle, "�� ����Ʈ�� �̹� ��ҵ� ����Ʈ�Դϴ�.");
        Debug.Assert(!p_IsComplete, "�� ����Ʈ�� �̹� �Ϸ�� ����Ʈ�Դϴ�.");
    }
}
