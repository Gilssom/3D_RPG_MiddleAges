using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using CharacterController;

public class PlayerInfo : BaseInfo
{
    public DataManager m_DataManager         { get; private set; }
    public WeaponManager m_WeaponManager     { get; private set; }
    public StateMachine stateMachine         { get; private set; }
    public Rigidbody m_Rigid                 { get; private set; }
    public Animator m_Anim                   { get; private set; }
    public CapsuleCollider m_CapsuleCollider { get; private set; }
    public UI_Inven m_UIInven                { get; private set; }

    [SerializeField]
    private Transform m_RightHand;

    #region #캐릭터 부가 스탯
    [Header("캐릭터 부가 스탯")]
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

    public int CriticalChance { get { return m_CriticalChance; } }
    public float CriticalDamage { get { return m_CriticalDamage; } }
    public int DashCount { get { return m_DashCount; } }
    public int Exp 
    { 
        get { return m_Exp; } 
        set 
        { 
            m_Exp = value;

            // 레벨업 체크

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
                SetStat(level);
            }
        } 
    }
    public int Gold { get { return m_Gold; } set { m_Gold = value; } }
    public int Fragments { get { return m_Fragments; } set { m_Fragments = value; } }

    [Header("대쉬기 옵션")]
    [SerializeField, Tooltip("Dash 파워")]
    public float m_DashPower;
    [SerializeField, Tooltip("Dash 앞구르기 모션 시간")]
    public float m_DashRollTime;
    [SerializeField, Tooltip("Dash 재입력 시간")]
    public float m_DashReInputTime;
    [SerializeField, Tooltip("Dash 경직 시간")]
    public float m_DashTetanyTime;
    [SerializeField, Tooltip("Dash 재사용 대기시간")]
    public float m_DashCoolTime;
    #endregion

    #region 플레이어 방어구 및 무기 강화 수치
    [SerializeField, Tooltip("머리장식 강화 단계")]
    public int m_HelmatLevel;
    [SerializeField, Tooltip("어깨 강화 단계")]
    public int m_ShoulderLevel;
    [SerializeField, Tooltip("상의 강화 단계")]
    public int m_TopLevel;
    [SerializeField, Tooltip("하의 강화 단계")]
    public int m_BottomLevel;
    [SerializeField, Tooltip("장갑 강화 단계")]
    public int m_GloveLevel;
    [SerializeField, Tooltip("무기 강화 단계")]
    public int m_WeaponLevel;
    #endregion

    #region #Sciprtable Object Data
    [Header("이펙트 데이터")]
    public EffectData m_EffectData;
    [Header("이펙트 리스트")]
    public VisualEffect[] m_EffectList;
    #endregion

    protected override void Init()
    {
        playerInfo = this;
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
        SetStat(m_Level);
    }

    protected override void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = m_DataManager.StatDict;
        Data.Stat stat = dict[level];

        m_Hp = stat.maxHp;
        m_MaxHp = stat.maxHp;
        m_Attack = stat.attack;
        m_CriticalChance = stat.criticalchance;
        m_CriticalDamage = stat.criticaldamage;
        m_Defense = stat.defense;
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
        Player playerCtrl = GetComponent<Player>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(playerCtrl));
        stateMachine.AddState(StateName.DASH, new DashState(playerCtrl, m_DashPower, m_DashTetanyTime, m_DashCoolTime));
        stateMachine.AddState(StateName.ATTACK, new AttackState(playerCtrl));
    }
}
