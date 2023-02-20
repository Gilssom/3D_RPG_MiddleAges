using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven : UI_Popup
{
    enum GameObjects
    {
        ItemPanel,
        UI_Inven_GridArea
    }

    enum Buttons
    {

    }

    [SerializeField]
    private UI_Inven_Slot[] m_Slots;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

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
    }

    public void CloseInventory()
    {
        GetObject((int)GameObjects.ItemPanel).SetActive(false);
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

    public int SearchSlotItem(string itemName)
    {
        for (int i = 0; i < m_Slots.Length; i++)
        {
            if (m_Slots[i].m_Item != null)
            {
                if (itemName == m_Slots[i].m_Item.m_ItemName)
                    return m_Slots[i].m_ItemCount;
            }
        }

        return 0;
    }

    public void SetItemCount(string itemName, int itemCount)
    {
        for (int i = 0; i < m_Slots.Length; i++)
        {
            if (m_Slots[i].m_Item != null)
            {
                if (itemName == m_Slots[i].m_Item.m_ItemName)
                {
                    m_Slots[i].SetSlotCount(-itemCount);
                    return;
                }
            }
        }
    }

    #region #인벤토리 정렬 보류
    public void SortInven(PointerEventData data)
    {
        // 1. i = 가장 앞에 있는 빈칸을 찾는 포인트
        // 2. j = i 커서 위치를 기준으로 뒤로 이동하며 존재하는 아이템 슬롯을 찾는 포인트

        // 3. i 가 빈칸을 찾으면 j 커서는 i + 1 위치로 이동
        // 4. j 커서가 아이템을 찾으면 아이템을 i 커서로 옮김 > i 커서는 i + 1 로 옮김
        // 5. j 커서가 인벤토리 최대치에 도달하면 종료

        int i = -1;
        while (m_Slots[++i].m_Item != null) ; // ex) ++i = 0 => 0번째 칸이 비어 있지 않다면 아래 실행
        int j = i; 

        while (true)
        {
            // ex) ++j = i + 1 이 최대치 보다 작고, i + 1 번째 슬롯이 아이템이 있다면

            Debug.Log(j);
            Debug.Log(m_Slots.Length);
            Debug.Log(m_Slots[j].m_Item);
            while (++j < m_Slots.Length && m_Slots[j].m_Item == null) 
            {
                // i + 1 번째 슬롯이 마지막 슬롯이라면 중지
                if (j == m_Slots.Length)
                    break;

                // i 번째 슬롯에 i + 1 번째 슬롯 아이템을 가져오고
                m_Slots[i].m_Item = m_Slots[j].m_Item;
                // i + 1 번째 슬롯을 빈 슬롯으로 만들어주고
                m_Slots[j].m_Item = null;
                // i++ 을 해서 다음 슬롯에 대한 연산 재시작
                i++;
            }
        }
    }
    #endregion
}