using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class EnemyInfo : BaseInfo
{
    public DataManager m_DataManager            { get; private set; }
    public StateMachine stateMachine            { get; private set; }
    public Rigidbody m_Rigid                    { get; private set; }
    public Animator m_Anim                      { get; private set; }
    public CapsuleCollider m_CapsuleCollider    { get; private set; }

    #region #캐릭터 부가 스탯
    [Header("몬스터 부가 스탯")]
    [SerializeField]
    string m_name;
    [SerializeField]
    int m_Id;
    [SerializeField]
    int m_DropExp;
    [SerializeField]
    int m_DropGold;

    public string Name { get { return m_name; } set { m_name = value; } }
    public int ID { get { return m_Id; } set { m_Id = value; } }
    public int DropExp { get { return m_DropExp; } set { m_DropExp = value; } }
    public int DropGold { get { return m_DropGold; } set { m_DropGold = value; } }

    [Header("공격 옵션")]
    [SerializeField, Tooltip("플레이어 감지 유무")]
    bool m_CheckPlayer;
    [SerializeField, Tooltip("공격 가능 유무")]
    bool m_ReadyAttack;
    [SerializeField, Tooltip("공격 재사용 대기시간")]
    float m_AttackCoolTime;
    [SerializeField, Tooltip("공격 실행 범위")]
    float m_AttackRange;
    [SerializeField, Tooltip("플레이어 탐지 범위")]
    float m_ScanRange;

    public bool CheckPlayer { get { return m_CheckPlayer; } set { m_CheckPlayer = value; } }
    public bool ReadyAttack { get { return m_ReadyAttack; } set { m_ReadyAttack = value; } }
    public float ScanRange      { get { return m_ScanRange; } set { m_ScanRange = value; } }
    public float AttackRange    { get { return m_AttackRange; } set { m_AttackRange = value; } }
    public float AttackCoolTime { get { return m_AttackCoolTime; } set { m_AttackCoolTime = value; } }
    #endregion

    protected override void Init()
    {
        m_DataManager = new DataManager();
        m_Rigid = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        InitStateMachine();
        m_DataManager.Init("MonsterStatData");
        SetStat(m_Id);
    }

    protected override void SetStat(int id)
    {
        Dictionary<int, Data.MonsterStat> dict = m_DataManager.MonsterStatDict;
        Data.MonsterStat stat = dict[id];

        m_Level = stat.level;
        m_Hp = stat.maxHp;
        m_MaxHp = stat.maxHp;
        m_Attack = stat.attack;
        m_Defense = stat.defense;
        m_DropExp = stat.dropExp;
        m_DropGold = stat.dropGold;
        m_name = stat.name;
    }

    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    public void OnUpdateStat(float moveSpeed)
    {
        this.m_Speed = moveSpeed;
    }

    protected override void InitStateMachine()
    {
        Enemy enemyCtrl = GetComponent<Enemy>();
        stateMachine = new StateMachine(StateName.IDLE, new EnemyIdleState(enemyCtrl));
        stateMachine.AddState(StateName.MOVE, new EnemyMoveState(enemyCtrl));
        stateMachine.AddState(StateName.ATTACK, new EnemyAttackState(enemyCtrl));
        stateMachine.AddState(StateName.DIE, new EnemyDieState(enemyCtrl));
    }
}
