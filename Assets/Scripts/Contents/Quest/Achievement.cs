using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Achievement", fileName = "Achievement_")]
public class Achievement : Quest
{
    public override bool p_IsCancelable => false;

    // �������� ���� ������ ���̺� �Ǿ�� �ϱ� ������
    public override bool p_IsSavable => true; 

    public override void Cancel()
    {
        Debug.LogAssertion("����� �� ���� �����Դϴ�.");
    }
}
