using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Popup
{
    enum GameObjects
    {
        ItemPanel,
        UI_Inven_GridArea
    }

    [SerializeField]
    private UI_Inven_Slot[] m_Slots;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.UI_Inven_GridArea);

        foreach (Transform child in gridPanel.transform)
            ResourcesManager.Instance.Destroy(child.gameObject);

        // 실제 인벤토리 정보를 참고해야함
        for (int i = 0; i < 63; i++)
        {
            UIManager.Instance.MakeSubItem<UI_Inven_Slot>(parent: gridPanel.transform);
        }

        m_Slots = gridPanel.GetComponentsInChildren<UI_Inven_Slot>();

        GetObject((int)GameObjects.ItemPanel).SetActive(false);
    }

    public void OpenInventory()
    {
        GetObject((int)GameObjects.ItemPanel).SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseInventory()
    {
        GetObject((int)GameObjects.ItemPanel).SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AcquireItem(Item item, int count = 1)
    {
        if (Item.ItemType.Equipment != item.m_ItemType)
        {
            for (int i = 0; i < m_Slots.Length; i++)
            {
                if (m_Slots[i].m_Item != null)
                {
                    if (m_Slots[i].m_Item.m_ItemName == item.m_ItemName)
                    {
                        m_Slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < m_Slots.Length; i++)
        {
            if (m_Slots[i].m_Item == null)
            {
                m_Slots[i].AddItem(item, count);
                return;
            }
        }
    }
}