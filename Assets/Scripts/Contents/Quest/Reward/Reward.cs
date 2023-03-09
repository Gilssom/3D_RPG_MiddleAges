using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField]
    private Sprite m_Icon;
    [SerializeField]
    private string m_Description;
    [SerializeField]
    private int m_Quantity;

    public Sprite p_Icon => m_Icon;
    public string p_Description => m_Description;
    public int p_Quantity => m_Quantity;

    public abstract void Give(Quest quest);
}
