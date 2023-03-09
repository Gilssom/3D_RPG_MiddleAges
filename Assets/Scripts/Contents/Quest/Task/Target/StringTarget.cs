using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지정한 타겟을 string 을 이용해 맞는지 아닌지를 판별
[CreateAssetMenu(menuName = "Quest/Task/Target/String", fileName = "Target_")]
public class StringTarget : TaskTarget
{
    [SerializeField]
    private string m_Value;

    public override object Value => m_Value;

    public override bool IsEqual(object target)
    {
        string targetAsString = target as string;

        if (targetAsString == null)
            return false;
        return m_Value == targetAsString;
    }
}
