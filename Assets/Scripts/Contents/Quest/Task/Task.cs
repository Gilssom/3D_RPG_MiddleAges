using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactivem,
    Running,
    Complete,
}

[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    // ������ Update �� ������ ��ȯ���� �ʰ� event �Լ��� �������ν� ���� ������ ���ִ� �̺�Ʈ
    #region #Events
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion
    [SerializeField]
    private Category m_Category;

    [Header("���� ���� ����")]
    [SerializeField]
    private string m_CodeName;
    [SerializeField]
    private string m_Description;

    [Header("���� ���� ī��Ʈ ���")]
    [SerializeField]
    private TaskAction m_Action;

    [Header("���� ���� ī��Ʈ ���")]
    // �������� Ÿ���� Task�� �������� �ֱ� ������ �迭
    // ex) ���� ��� ���� 10������ �������� óġ�ض�.
    [SerializeField] 
    private TaskTarget[] m_Targets;

    [Header("���� ���� ���� ���� �ɼ�")]
    [SerializeField]
    private InitialSuccessValue m_InitialSuccessValue;
    [SerializeField] // �����ϱ� ���� �ʿ��� ���� Ƚ��
    private int m_NeedSuccessToComplete;
    [SerializeField] // Task �ϷῩ�� ������� ���� Ƚ���� ���� ���� ������?
    private bool m_CanReceiveReportsDuringComplete;

    private TaskState m_State;
    public int m_CurrentSuccess;

    public event StateChangedHandler onStateChanged;
    public event SuccessChangedHandler onSuccessChanged;

    // ���� ������ Ƚ��
    public int p_CurrentSucceses
    {
        get => m_CurrentSuccess;
        set
        {
            int prevSuccess = m_CurrentSuccess;
            m_CurrentSuccess = Mathf.Clamp(value, 0, m_NeedSuccessToComplete);

            if (m_CurrentSuccess != prevSuccess)
            {
                State = m_CurrentSuccess == m_NeedSuccessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, m_CurrentSuccess, prevSuccess);
            }
        }
    }

    // p_ :: Property
    public Category p_Category => m_Category;
    public string p_CodeName => m_CodeName;
    public string p_Description => m_Description;
    public int p_NeedSuccessToComplete => m_NeedSuccessToComplete;

    public TaskState State
    {
        get => m_State;
        set
        {
            var prevState = m_State;
            m_State = value;
            // ?. => �� ������ null �̸� null �� ��ȯ / null �� �ƴ϶�� �ڿ� �Լ��� ����
            onStateChanged?.Invoke(this, m_State, prevState);
        }
    }

    public bool IsComplete => State == TaskState.Complete;

    // �ش� ����Ʈ�� ����
    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        State = TaskState.Running;
        if (m_InitialSuccessValue)
            p_CurrentSucceses = m_InitialSuccessValue.GetValue(this);
    }

    public void End()
    {
        onStateChanged = null;
        onSuccessChanged = null;
    }

    public void ReceiveReport(int successCount)
    {
        // Module�� ��� �� ��ȯ
        p_CurrentSucceses = m_Action.Run(this, m_CurrentSuccess, successCount);
    }

    public void Complete()
    {
        p_CurrentSucceses = m_NeedSuccessToComplete;
    }

    // TaskTarget �� ���� Task�� ���� Ƚ���� ���� ���� ������� Ȯ���ϴ� �Լ�
    // Setting �س��� Target�� �߿� �ش��ϴ� Target�� �ִٸ� true, ������ false �� ��ȯ
    public bool IsTarget(string category, object target)
        => p_Category == category 
        && m_Targets.Any(x => x.IsEqual(target))
        && (!IsComplete || (IsComplete && m_CanReceiveReportsDuringComplete));
}
