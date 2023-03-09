using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ Ÿ���� string �� �̿��� �´��� �ƴ����� �Ǻ�
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
