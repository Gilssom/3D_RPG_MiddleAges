using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject
{
    [SerializeField]
    private string m_Description;

    public abstract bool IsPass(Quest quest);
}
