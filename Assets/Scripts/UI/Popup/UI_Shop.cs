using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : UI_Popup
{
    public ShopType m_ShopType;

    public enum ShopType
    {
        Potion,
        Enforce,
    }

    enum GameObjects
    {
        ShopPanel,
        UI_Shop_GridArea
    }

    [SerializeField]
    private Item[] m_ItemInfo;

    [SerializeField]
    private UI_Shop_Slot[] m_Slots;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.UI_Shop_GridArea);

        foreach (Transform child in gridPanel.transform)
            ResourcesManager.Instance.Destroy(child.gameObject);

        // 실제 아이템 판매 정보를 참고해야함
        switch (m_ShopType)
        {
            case ShopType.Potion:
                for (int i = 0; i < ShopManager.Instance.m_ShopPotionItems.Length; i++)
                {
                    UIManager.Instance.MakeSubItem<UI_Shop_Slot>(parent: gridPanel.transform);
                }
                break;
            case ShopType.Enforce:
                for (int i = 0; i < ShopManager.Instance.m_ShopEnforceItems.Length; i++)
                {
                    UIManager.Instance.MakeSubItem<UI_Shop_Slot>(parent: gridPanel.transform);
                }
                break;
        }

        m_Slots = gridPanel.GetComponentsInChildren<UI_Shop_Slot>();

        SetItemData();

        GetObject((int)GameObjects.ShopPanel).SetActive(false);
    }

    public void OpenPotionShop()
    {
        GetObject((int)GameObjects.ShopPanel).SetActive(true);
    }

    public void ClosePotionShop()
    {
        GetObject((int)GameObjects.ShopPanel).SetActive(false);
    }

    void SetItemData()
    {
        switch (m_ShopType)
        {
            case ShopType.Potion:
                for (int i = 0; i < m_Slots.Length; i++)
                {
                    m_Slots[i].SetData(ShopManager.Instance.m_ShopPotionItems[i].m_ItemName,
                        ShopManager.Instance.m_ShopPotionItems[i].m_AddPrice,
                        ShopManager.Instance.m_ShopPotionItems[i].m_AddLevel,
                        m_ItemInfo[i]);
                }
                break;
            case ShopType.Enforce:
                for (int i = 0; i < m_Slots.Length; i++)
                {
                    m_Slots[i].SetData(ShopManager.Instance.m_ShopEnforceItems[i].m_ItemName,
                        ShopManager.Instance.m_ShopEnforceItems[i].m_AddPrice,
                        ShopManager.Instance.m_ShopEnforceItems[i].m_AddLevel,
                        m_ItemInfo[i + ShopManager.Instance.m_ShopPotionItems.Length]);
                }
                break;
        }      
    }
}
