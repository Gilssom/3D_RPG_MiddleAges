using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Achievement", fileName = "Achievement_")]
public class Achievement : Quest
{
    public override bool p_IsCancelable => false;

    // 업적같은 경우는 무조건 세이브 되어야 하기 때문에
    public override bool p_IsSavable => true; 

    public override void Cancel()
    {
        Debug.LogAssertion("취소할 수 없는 업적입니다.");
    }
}
