using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Exp", fileName = "ExpReward_")]
public class ExpReward : Reward
{
    public override void Give(Quest quest)
    {
        BaseInfo.playerInfo.Exp += p_Quantity;
        PlayerPrefs.SetInt("BonusExp", p_Quantity);
        PlayerPrefs.Save();
    }
}
