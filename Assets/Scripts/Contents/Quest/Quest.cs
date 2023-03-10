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
    WaitingForComplete, // >> 자동 완료 퀘스트가 아닌 직접 완료를 시켜야 되는 형태의 퀘스트
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

    [Header("퀘스트 정보 작성")]
    [SerializeField]
    private string m_CodeName;
    [SerializeField]
    private string m_DisPlayName;
    [SerializeField, TextArea]
    private string m_Description;

    // 1. 종류 상관 없이 슬라임을 10마리 잡아라
    // 2. 레드 슬라임을 10마리 잡아라
    // 2. 블루 슬라임을 10마리 잡아라 -> 골룸을 5마리 잡아라
    // 여러개의 Task 가 들어가 있는 상황이 있을 수 있다. -> Task Group 이 필요
    [Header("퀘스트의 내용")]
    [SerializeField]
    private TaskGroup[] m_TaskGroup;

    [Header("퀘스트 보상")]
    [SerializeField]
    private Reward[] m_Rewards;

    [Header("퀘스트 옵션")]
    [SerializeField]
    private bool m_UseAutoComplete; // 자동 완료 퀘스트인지
    [SerializeField]
    private bool m_IsCancelable; // 취소할 수 없는 퀘스트인지
    // 세이브를 시킬 퀘스트인지 =
    //          튜토리얼의 가이드 퀘스트와 비슷하거나 같은 경우는 저장이 되어야 할 필요가 없기 때문
    [SerializeField]
    private bool m_IsSavable; 

    [Header("퀘스트 수행 및 취소 조건")]
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

    // Awake 역활의 함수 > Quest 가 System 에 등록되었을 때 실행
    public void OnRegister()
    {
        // Assert :: 인자로 들어온 값이 false면 뒤의 문장을 Error 로 띄워준다.
        // 절대 일어나서는 안되는 조건이 일어났을 때 검출하기 위한 코드
        // Debugging 코드로써 게임을 Build 해서 뽑아내면 Assert Code 는 무시가 된다. > 성능에 영향 X > 방어적 프로그래밍
        Debug.Assert(!p_IsRegistered, "이 퀘스트는 이미 등록된 퀘스트입니다.");

        foreach (var taskGroup in m_TaskGroup)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.p_Tasks)
                task.onSuccessChanged += OnSuccessChanged; // CallBack
        }

        m_State = QuestState.Running;
        p_CurrentTaskGroup.Start();
    }

    // 보고를 받는 함수
    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(p_IsRegistered, "이 퀘스트는 이미 등록된 퀘스트입니다.");
        Debug.Assert(!p_IsCancle, "이 퀘스트는 이미 취소된 퀘스트입니다.");

        if (p_IsComplete)
            return; // 퀘스트가 완료가 되었어도 보고를 받을 수 있는 상황이 있을 수 있기 때문

        p_CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (p_CurrentTaskGroup.p_IsAllTaskComplete)
        {
            if (m_CurrentTaskGroupIndex + 1 == m_TaskGroup.Length)
            {
                m_State = QuestState.WaitingForComplete;
                if (m_UseAutoComplete)
                    Complete();
            }
            else // 다음 TaskGroup 이 존재한다면
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

    // 퀘스트를 완료하는 함수
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

    // 퀘스트를 취소하는 함수
    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(p_IsCancelable, "이 퀘스트는 포기할 수 없는 퀘스트입니다.");

        m_State = QuestState.Cancel;
        onCanceled?.Invoke(this);
    }

    // Quest System 에 넘겨줄 Quest 복사본 :: Cloning
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

    // Conditional Attribute 는 인자로 전달한 Simbol 값이 선언되어 있으면
    // 함수를 실행하고 아니라면 함수를 무시하게 해줌 >> using System.Diagnostics; 선언
    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(p_IsRegistered, "이 퀘스트는 이미 등록된 퀘스트입니다.");
        Debug.Assert(!p_IsCancle, "이 퀘스트는 이미 취소된 퀘스트입니다.");
        Debug.Assert(!p_IsComplete, "이 퀘스트는 이미 완료된 퀘스트입니다.");
    }
}
