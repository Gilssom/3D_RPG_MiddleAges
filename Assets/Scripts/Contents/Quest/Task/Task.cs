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
    // 일일이 Update 로 정보를 변환하지 않고 event 함수를 만듦으로써 정보 갱신을 해주는 이벤트
    #region #Events
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion
    [SerializeField]
    private Category m_Category;

    [Header("조건 수행 내용")]
    [SerializeField]
    private string m_CodeName;
    [SerializeField]
    private string m_Description;

    [Header("수행 진행 카운트 방법")]
    [SerializeField]
    private TaskAction m_Action;

    [Header("수행 진행 카운트 대상")]
    // 여러개의 타겟을 Task에 넣을수도 있기 때문에 배열
    // ex) 종류 상관 없이 10마리의 슬라임을 처치해라.
    [SerializeField] 
    private TaskTarget[] m_Targets;

    [Header("수행 진행 관련 세팅 옵션")]
    [SerializeField]
    private InitialSuccessValue m_InitialSuccessValue;
    [SerializeField] // 성공하기 위해 필요한 성공 횟수
    private int m_NeedSuccessToComplete;
    [SerializeField] // Task 완료여부 상관없이 성공 횟수를 보고 받을 것인지?
    private bool m_CanReceiveReportsDuringComplete;

    private TaskState m_State;
    public int m_CurrentSuccess;

    public event StateChangedHandler onStateChanged;
    public event SuccessChangedHandler onSuccessChanged;

    // 현재 성공한 횟수
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
            // ?. => 이 변수가 null 이면 null 을 반환 / null 이 아니라면 뒤에 함수를 실행
            onStateChanged?.Invoke(this, m_State, prevState);
        }
    }

    public bool IsComplete => State == TaskState.Complete;

    // 해당 퀘스트의 주인
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
        // Module식 결과 값 변환
        p_CurrentSucceses = m_Action.Run(this, m_CurrentSuccess, successCount);
    }

    public void Complete()
    {
        p_CurrentSucceses = m_NeedSuccessToComplete;
    }

    // TaskTarget 을 통해 Task가 성공 횟수를 보고 받을 대상인지 확인하는 함수
    // Setting 해놓은 Target들 중에 해당하는 Target이 있다면 true, 없으면 false 를 반환
    public bool IsTarget(string category, object target)
        => p_Category == category 
        && m_Targets.Any(x => x.IsEqual(target))
        && (!IsComplete || (IsComplete && m_CanReceiveReportsDuringComplete));
}
