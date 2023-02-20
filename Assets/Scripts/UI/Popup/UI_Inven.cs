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

        // ���� �κ��丮 ������ �����ؾ���
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

    #region #�κ��丮 ���� ����
    public void SortInven(PointerEventData data)
    {
        // 1. i = ���� �տ� �ִ� ��ĭ�� ã�� ����Ʈ
        // 2. j = i Ŀ�� ��ġ�� �������� �ڷ� �̵��ϸ� �����ϴ� ������ ������ ã�� ����Ʈ

        // 3. i �� ��ĭ�� ã���� j Ŀ���� i + 1 ��ġ�� �̵�
        // 4. j Ŀ���� �������� ã���� �������� i Ŀ���� �ű� > i Ŀ���� i + 1 �� �ű�
        // 5. j Ŀ���� �κ��丮 �ִ�ġ�� �����ϸ� ����

        int i = -1;
        while (m_Slots[++i].m_Item != null) ; // ex) ++i = 0 => 0��° ĭ�� ��� ���� �ʴٸ� �Ʒ� ����
        int j = i; 

        while (true)
        {
            // ex) ++j = i + 1 �� �ִ�ġ ���� �۰�, i + 1 ��° ������ �������� �ִٸ�

            Debug.Log(j);
            Debug.Log(m_Slots.Length);
            Debug.Log(m_Slots[j].m_Item);
            while (++j < m_Slots.Length && m_Slots[j].m_Item == null) 
            {
                // i + 1 ��° ������ ������ �����̶�� ����
                if (j == m_Slots.Length)
                    break;

                // i ��° ���Կ� i + 1 ��° ���� �������� ��������
                m_Slots[i].m_Item = m_Slots[j].m_Item;
                // i + 1 ��° ������ �� �������� ������ְ�
                m_Slots[j].m_Item = null;
                // i++ �� �ؼ� ���� ���Կ� ���� ���� �����
                i++;
            }
        }
    }
    #endregion
}