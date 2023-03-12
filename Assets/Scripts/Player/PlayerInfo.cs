using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using CharacterController;
using UnityEngine.Events;

public class PlayerInfo : BaseInfo
{
    public DataManager m_DataManager         { get; private set; }
    public WeaponManager m_WeaponManager     { get; private set; }
    public StateMachine stateMachine         { get; private set; }
    public Rigidbody m_Rigid                 { get; private set; }
    public Animator m_Anim                   { get; private set; }
    public CapsuleCollider m_CapsuleCollider { get; private set; }
    public UI_Inven m_UIInven                { get; private set; }
    public Player m_Player;
    public UnityEvent onLevelUp;

    [SerializeField]
    private Transform m_RightHand;

    #region #ĳ���� �ΰ� ����
    [Header("ĳ���� �ΰ� ����")]
    [SerializeField]
    int m_CriticalChance;
    [SerializeField]
    float m_CriticalDamage;
    [SerializeField]
    protected int m_DashCount;
    [SerializeField]
    int m_Exp;
    [SerializeField]
    int m_Gold;
    [SerializeField]
    int m_Fragments;

    public int CriticalChance { get { return m_CriticalChance; } set { m_CriticalChance = value; } }
    public float CriticalDamage { get { return m_CriticalDamage; } set { m_CriticalDamage = value; } }
    public int DashCount { get { return m_DashCount; } }
    public int Exp 
    { 
        get { return m_Exp; } 
        set 
        { 
            m_Exp = value;

            // ������ üũ

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if(m_DataManager.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (m_Exp < stat.totalExp)
                    break;
                level++;
            }

            if(level != Level)
            {
                Debug.Log("Level Up!");
                Level = level;
                onLevelUp.Invoke();
                SetStat(level);
            }
        } 
    }
    public int Gold { get { return m_Gold; } set { m_Gold = value; } }
    public int Fragments { get { return m_Fragments; } set { m_Fragments = value; } }

    [Header("�뽬�� �ɼ�")]
    [SerializeField, Tooltip("Dash �Ŀ�")]
    public float m_DashPower;
    [SerializeField, Tooltip("Dash �ձ����� ��� �ð�")]
    public float m_DashRollTime;
    [SerializeField, Tooltip("Dash ���Է� �ð�")]
    public float m_DashReInputTime;
    [SerializeField, Tooltip("Dash ���� �ð�")]
    public float m_DashTetanyTime;
    [SerializeField, Tooltip("Dash ���� ���ð�")]
    public float m_DashCoolTime;
    #endregion

    #region �÷��̾� �� �� ���� ��ȭ ��ġ
    [Header("ĳ���� ��ȭ ��ġ")]
    [SerializeField, Tooltip("�Ӹ���� ��ȭ �ܰ�")]
    public int m_HelmatLevel;
    [SerializeField, Tooltip("��� ��ȭ �ܰ�")]
    public int m_ShoulderLevel;
    [SerializeField, Tooltip("���� ��ȭ �ܰ�")]
    public int m_TopLevel;
    [SerializeField, Tooltip("���� ��ȭ �ܰ�")]
    public int m_BottomLevel;
    [SerializeField, Tooltip("�尩 ��ȭ �ܰ�")]
    public int m_GloveLevel;
    [SerializeField, Tooltip("���� ��ȭ �ܰ�")]
    public int m_WeaponLevel;
    #endregion

    #region �÷��̾� �ɷ�ġ ��ȭ ��ġ
    [Header("ĳ���� �ɷ�ġ ��ȭ ��ġ")]
    [SerializeField, Tooltip("�ִ� ü�� �߰� �ɷ�ġ")]
    public int m_Plus_MaxHp;
    [SerializeField, Tooltip("����� ��� �ɷ�ġ")]
    public int m_Plus_IncreaseHp;
    [SerializeField, Tooltip("���� �߰� �ɷ�ġ")]
    public int m_Plus_Defense;
    [SerializeField, Tooltip("ũƼ���� Ȯ�� �߰� �ɷ�ġ")]
    public int m_Plus_CriChance;
    [SerializeField, Tooltip("ũ��Ƽ�� ������ �߰� �ɷ�ġ")]
    public float m_Plus_CriDamage;
    [SerializeField, Tooltip("���ݷ� �߰� �ɷ�ġ")]
    public int m_Plus_Attack;
    #endregion

    #region #Sciprtable Object Data
    [Header("����Ʈ ������")]
    public EffectData m_EffectData;
    [Header("����Ʈ ����Ʈ")]
    public VisualEffect[] m_EffectList;
    public GameObject[] m_EffectList_Obj;
    #endregion

    protected override void Init()
    {
        playerInfo = this;
        m_Player = GetComponent<Player>();
        Debug.Log($"{name} - {playerInfo}");
        m_UIInven = new UI_Inven();
        m_DataManager = new DataManager();
        m_WeaponManager = new WeaponManager(m_RightHand);
        m_WeaponManager.m_UnRegisterWeapon = (weapon) => { Destroy(weapon); };

        m_Rigid = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        InitStateMachine();
        m_DataManager.Init("StatData");

        // Test
        m_Level = 1;
        m_Speed = 1;
        m_DashCount = 2;
        m_Exp = 0;
        m_Gold = 0;
        m_Fragments = 0;
        m_Plus_IncreaseHp = 5;
        SetStat(m_Level);
    }

    protected override void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = m_DataManager.StatDict;
        Data.Stat stat = dict[level];

        m_Hp += stat.maxHp;
        m_MaxHp += stat.maxHp;
        m_Attack += stat.attack;
        m_CriticalChance += stat.criticalchance;
        m_CriticalDamage += stat.criticaldamage;
        m_Defense += stat.defense;

        foreach (NMPassiveSkill skill in SkillManager.Instance.NMPassiveSkills)
        {
            skill.SkillExecute();
        }
    }

    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    public void OnUpdateStat(float moveSpeed, int dashCount)
    {
        this.m_Speed = moveSpeed;
        this.m_DashCount = dashCount;
    }

    protected override void InitStateMachine()
    {
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(m_Player));
        stateMachine.AddState(StateName.DASH, new DashState(m_Player, m_DashPower, m_DashTetanyTime, m_DashCoolTime));
        stateMachine.AddState(StateName.ATTACK, new AttackState(m_Player));
    }
}
