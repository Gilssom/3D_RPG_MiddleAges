using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfo : SingletomManager<PlayerInfo>
{
    protected BaseInfo() { }

    #region #���� ����
    [Header("���� ����")]
    [SerializeField]
    protected int m_Level;
    [SerializeField]
    protected int m_Hp;
    [SerializeField]
    protected int m_MaxHp;
    [SerializeField]
    protected int m_Attack;
    [SerializeField]
    protected int m_Defense;
    [SerializeField]
    protected float m_Speed;

    public int Level { get { return m_Level; } set { m_Level = value; } }
    public int Hp { get { return m_Hp; } set { m_Hp = value; } }
    public int MaxHp { get { return m_MaxHp; } set { m_MaxHp = value; } }
    public int Attack { get { return m_Attack; } set { m_Attack = value; } }
    public int Defense { get { return m_Defense; } set { m_Defense = value; } }
    public float MoveSpeed { get { return m_Speed; } set { m_Speed = value; } }
    #endregion

    private void Start()
    {
        Init();
    }

    protected virtual void Init() { }
    protected virtual void SetStat(int number) { }
    protected virtual void InitStateMachine() { }
}
