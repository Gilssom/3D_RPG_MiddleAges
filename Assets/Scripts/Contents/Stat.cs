using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    int m_Level;
    [SerializeField]
    int m_Hp;
    [SerializeField]
    int m_MaxHp;

    [SerializeField]
    int m_Attack;
    [SerializeField]
    int m_Defense;

    [SerializeField]
    float m_MoveSpeed;

    public int Level    { get { return m_Level; }   set { m_Level = value; } }
    public int Hp       { get { return m_Hp; }      set { m_Hp = value; } }
    public int MaxHp    { get { return m_MaxHp; }   set { m_MaxHp = value; } }
    public int Attack   { get { return m_Attack; }  set { m_Attack = value; } }
    public int Defense  { get { return m_Defense; } set { m_Defense = value; } }
    public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

    private void Start()
    {
        m_Level = 1;
        m_Hp = 100;
        m_MaxHp = m_Hp;
        m_Attack = 10;
        m_Defense = 2;
        m_MoveSpeed = 1;
    }

    //public virtual void OnAttacked(Stat attacker)
    //{
    //    int damage = Mathf.Max(0, attacker.Attack - playerInfo.Defense);
    //    playerInfo.Hp -= damage;

    //    if (playerInfo.Hp <= 0)
    //    {
    //        playerInfo.Hp = 0;
    //        //OnDead();
    //    }
    //}

    //protected virtual void OnDead(Stat attacker)
    //{
    //    playerInfo.Exp += 20;
    //}
}
