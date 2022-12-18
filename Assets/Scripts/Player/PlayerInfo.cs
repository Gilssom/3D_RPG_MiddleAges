using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance { get { return instance; } }
    public StateMachine stateMachine         { get; private set; }
    public Rigidbody m_Rigid                 { get; private set; }
    public Animator m_Anim                   { get; private set; }
    public CapsuleCollider m_CapsuleCollider { get; private set; }

    private static PlayerInfo instance;

    public float MoveSpeed { get { return m_Speed; } }
    public int DashCount { get { return m_DashCount; } }

    [Header("Ä³¸¯ÅÍ ½ºÅÈ")]
    [SerializeField]
    protected float m_Speed;
    [SerializeField]
    protected int m_DashCount;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            m_Rigid = GetComponent<Rigidbody>();
            m_Anim = GetComponent<Animator>();
            m_CapsuleCollider = GetComponent<CapsuleCollider>();

            DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    void Start()
    {
        InitStateMachine();
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

    public void InitStateMachine()
    {
        Player playerCtrl = GetComponent<Player>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(playerCtrl));
    }
}
