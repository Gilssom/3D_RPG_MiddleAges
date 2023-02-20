using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopPotionItemData
{
    // 키값 , 부위 , 수치
    public int m_ItemId;
    public string m_ItemName;
    public int m_AddPrice;
    public int m_AddLevel;
}

[System.Serializable]
public class ShopEnforceItemData
{
    // 키값 , 부위 , 수치
    public int m_ItemId;
    public string m_ItemName;
    public int m_AddPrice;
    public int m_AddLevel;
}

[System.Serializable]
public class ShopSaleItemData
{
    // 키값 , 부위 , 수치
    public int m_ItemId;
    public string m_ItemName;
    public int m_SalePrice;
}

public class ShopManager : SingletomManager<ShopManager>
{
    [SerializeField]
    public ShopPotionItemData[] m_ShopPotionItems;
    [SerializeField]
    public ShopEnforceItemData[] m_ShopEnforceItems;
    [SerializeField]
    public ShopSaleItemData[] m_ShopSaleItems;

    public DataManager m_DataManager { get; private set; }

    void Awake()
    {
        m_DataManager = new DataManager();

        m_DataManager.Init("ItemBaseData");
        SetPotionItemData();
        SetEnforceItemData();
        SetSaleItemData();
    }

    // 아이템의 종류가 정해져 있다보니
    // 크기가 정해진 배열을 사용하는게 맞는거 같음.
    void SetPotionItemData()
    {
        for (int i = 0; i < m_ShopPotionItems.Length; i++)
        {
            Dictionary<int, Data.ItemStat> dict = m_DataManager.ItemDict;
            Data.ItemStat stat = dict[i + 1000];

            m_ShopPotionItems[i].m_ItemId = stat.itemId;
            m_ShopPotionItems[i].m_ItemName = stat.itemName;
            m_ShopPotionItems[i].m_AddPrice = stat.itemAddPrice;
            m_ShopPotionItems[i].m_AddLevel = stat.itemAddLevel;
        }
    }

    void SetEnforceItemData()
    {
        for (int i = 0; i < m_ShopEnforceItems.Length; i++)
        {
            Dictionary<int, Data.ItemStat> dict = m_DataManager.ItemDict;
            Data.ItemStat stat = dict[i + 2000];

            m_ShopEnforceItems[i].m_ItemId = stat.itemId;
            m_ShopEnforceItems[i].m_ItemName = stat.itemName;
            m_ShopEnforceItems[i].m_AddPrice = stat.itemAddPrice;
            m_ShopEnforceItems[i].m_AddLevel = stat.itemAddLevel;
        }
    }

    void SetSaleItemData()
    {
        for (int i = 0; i < m_ShopPotionItems.Length; i++)
        {
            Dictionary<int, Data.ItemStat> dict = m_DataManager.ItemDict;
            Data.ItemStat stat = dict[i + 1000];

            m_ShopSaleItems[i].m_ItemId = stat.itemId;
            m_ShopSaleItems[i].m_ItemName = stat.itemName;
            m_ShopSaleItems[i].m_SalePrice = stat.itemSalePrice;
        }

        for (int i = m_ShopPotionItems.Length; i < m_ShopSaleItems.Length; i++)
        {
            Dictionary<int, Data.ItemStat> dict = m_DataManager.ItemDict;
            Data.ItemStat stat = dict[i + 2000 - m_ShopPotionItems.Length];

            m_ShopSaleItems[i].m_ItemId = stat.itemId;
            m_ShopSaleItems[i].m_ItemName = stat.itemName;
            m_ShopSaleItems[i].m_SalePrice = stat.itemSalePrice;
        }
    }
}
