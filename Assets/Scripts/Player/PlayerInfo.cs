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

    [SerializeField]
    private Transform m_RightHand;

    #region #캐릭터 부가 스탯
    [Header("캐릭터 부가 스탯")]
    [SerializeField]
    protected int m_DashCount;
    [SerializeField]
    int m_Exp;
    [SerializeField]
    int m_Gold;

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

    #region #Sciprtable Object Data
    [Header("이펙트 데이터")]
    public EffectData m_EffectData;
    #endregion

    protected override void Init()
    {
        playerInfo = this;
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
        SetStat(m_Level);
    }

    protected override void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = m_DataManager.StatDict;
        Data.Stat stat = dict[level];

        m_Hp = stat.maxHp;
        m_MaxHp = stat.maxHp;
        m_Attack = stat.attack;
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
