using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Item", fileName = "ItemReward_")]
public class ItemReward : Reward
{
    public override void Give(Quest quest)
    {
        InventoryManager.Instance.m_InvenBase.AcquireItem(p_Item, p_Quantity);
        //PlayerPrefs.Save();
    }
}
