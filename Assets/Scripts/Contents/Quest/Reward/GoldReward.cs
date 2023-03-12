using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Gold", fileName = "GoldReward_")]
public class GoldReward : Reward
{
    public override void Give(Quest quest)
    {
        BaseInfo.playerInfo.Gold += p_Quantity;
        PlayerPrefs.SetInt("BonusGold", p_Quantity);
        PlayerPrefs.Save();
    }
}
