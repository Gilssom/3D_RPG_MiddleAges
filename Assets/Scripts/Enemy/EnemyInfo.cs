using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using CharacterController;
using TinyScript;

public class EnemyInfo : BaseInfo
{
    public DataManager m_DataManager            { get; private set; }
    public StateMachine stateMachine            { get; private set; }
    public Rigidbody m_Rigid                    { get; private set; }
    public Animator m_Anim                      { get; private set; }
    public CapsuleCollider m_CapsuleCollider    { get; private set; }
    public Material m_Material                  { get; private set; }
    public LootDrop m_LootDrop                  { get; private set; }

    #region #���� �ΰ� ����
    [Header("���� �ΰ� ����")]
    [SerializeField]
    Defines.MonsterType m_Type;
    [SerializeField]
    string m_name;
    [SerializeField]
    int m_Id;
    [SerializeField]
    int m_DropExp;
    [SerializeField]
    int m_DropGold;

    public Defines.MonsterType Type { get { return m_Type; } set { m_Type = value; } }
    public string Name { get { return m_name; } set { m_name = value; } }
    public int ID { get { return m_Id; } set { m_Id = value; } }
    public int DropExp { get { return m_DropExp; } set { m_DropExp = value; } }
    public int DropGold { get { return m_DropGold; } set { m_DropGold = value; } }

    [Header("���� �ɼ�")]
    [SerializeField, Tooltip("�÷��̾� ���� ����")]
    bool m_CheckPlayer;
    [SerializeField, Tooltip("���� ���� ����")]
    bool m_ReadyAttack;
    [SerializeField, Tooltip("���� ���� ���ð�")]
    float m_AttackCoolTime;
    [SerializeField, Tooltip("���� ���� ����")]
    float m_AttackRange;
    [SerializeField, Tooltip("�÷��̾� Ž�� ����")]
    float m_ScanRange;

    public bool CheckPlayer { get { return m_CheckPlayer; } set { m_CheckPlayer = value; } }
    public bool ReadyAttack { get { return m_ReadyAttack; } set { m_ReadyAttack = value; } }
    public float ScanRange      { get { return m_ScanRange; } set { m_ScanRange = value; } }
    public float AttackRange    { get { return m_AttackRange; } set { m_AttackRange = value; } }
    public float AttackCoolTime { get { return m_AttackCoolTime; } set { m_AttackCoolTime = value; } }

    [Header("���� ��� ����")]
    [SerializeField, Tooltip("���� ��� Ȯ��")]
    float m_PotionChance;
    [SerializeField, Tooltip("���� ��� ����")]
    int m_PotionCount;
    [SerializeField, Tooltip("�ּ� ��ȭ���� ��� ����")]
    int m_MinFragmentsCount;
    [SerializeField, Tooltip("�ִ� ��ȭ���� ��� ����")]
    int m_MaxFragmentsCount;
    [SerializeField, Tooltip("������ ��� Ƚ��")]
    int m_RandomDropCount;
    [SerializeField, Tooltip("������ ��� ����")]
    float m_ItemDropRange;

    public float PotionChance { get { return m_PotionChance; } set { m_PotionChance = value; } }
    public int PotionCount  { get { return m_PotionCount; } set { m_PotionCount = value; } }
    public int MinFargCount { get { return m_MinFragmentsCount; } set { m_MinFragmentsCount = value; } }
    public int MaxFragCount { get { return m_MaxFragmentsCount; } set { m_MaxFragmentsCount = value; } }
    public int RandomDropCount { get { return m_RandomDropCount; } set { m_RandomDropCount = value; } }
    public float ItemDropRange { get { return m_ItemDropRange; } set { m_ItemDropRange = value; } }
    #endregion

    #region #����Ʈ
    [Header("����Ʈ ����Ʈ")]
    public VisualEffect[] m_EffectList;
    [Header("Dissolve ���� �Ӽ�")]
    [SerializeField, Tooltip("Dissolve ��� �ӵ�")]
    public float m_DissolveSpeed;
    [SerializeField, Tooltip("Dissolve ���� ��")]
    public float m_DissolveCutoff = default;
    #endregion

    protected override void Init()
    {
        enemyInfo = this;
        m_DataManager = new DataManager();
        m_Rigid = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_Material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        m_Type = (Defines.MonsterType)m_Id;
        m_LootDrop = ResourcesManager.Instance.Load<LootDrop>($"Data/Loot System/{m_Id}");

        m_CheckPlayer = false;
        m_ReadyAttack = true;

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

        m_PotionChance = stat.potionChance;
        m_PotionCount = stat.potionCount;
        m_MinFragmentsCount = stat.minFragments;
        m_MaxFragmentsCount = stat.maxFragments;
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
