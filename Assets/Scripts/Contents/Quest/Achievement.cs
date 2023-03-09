using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Achievement", fileName = "Achievement_")]
public class Achievement : Quest
{
    public override bool p_IsCancelable => false;

    public override void Cancel()
    {
        Debug.LogAssertion("취소할 수 없는 업적입니다.");
    }
}
