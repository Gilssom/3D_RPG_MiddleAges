using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Slot : UI_Base
{
    public Image m_SlotImage;
    public Text m_SlotCount;
    public Item m_Item;
    public int m_ItemCount;

    private Rect m_BaseRect;
    private UI_InputNumber m_InputNumber;
    public RectTransform m_QuickSlotBaseRect;

    public override void Init() 
    { 
        SetEvent();

        m_BaseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        m_InputNumber = InventoryManager.Instance.m_InputNumber;
        m_QuickSlotBaseRect = InventoryManager.Instance.m_QuickSlotBaseRect;
    }

    // ���� ������ �̹��� ����
    void SetColor(float alpha)
    {
        Color color = m_SlotImage.color;
        color.a = alpha;
        m_SlotImage.color = color;
    }

    // ������ �߰�
    public void AddItem(Item item, int count = 1)
    {
        m_Item = item;
        m_ItemCount = count;
        m_SlotImage.sprite = m_Item.m_ItemImage;

        if (m_Item.m_ItemType != Item.ItemType.Equipment)
            m_SlotCount.text = m_ItemCount.ToString();
        else
            m_SlotCount.text = null;

        SetColor(1);
    }

    // UI EventHandler ���
    void SetEvent()
    {
        // ������ Enter �̺�Ʈ
        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} ����!"); 
            if (m_Item != null)
                InventoryManager.Instance.ShowToolTip(m_Item, transform.position);
        }
        , Defines.UIEvent.Enter);

        // ������ Exit �̺�Ʈ
        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} ���� �Ⱥ���"); 
            InventoryManager.Instance.HideToolTip();
        }
        , Defines.UIEvent.Exit);

        // ������ ���� Ŭ�� �̺�Ʈ
        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} Ŭ��!");

            if (m_Item != null)
            {
                // ������ �Ҹ�
                ItemEffectDataBase.Instance.UseItem(m_Item);

                if (m_Item.m_ItemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
        );

        // ������ �巡�� �̺�Ʈ
        gameObject.BindEvent((PointerEventData) =>
        {
            if (m_Item != null)
            {
                //Debug.Log($"{m_Item.m_ItemName} �巡�� ����!");

                UI_DragSlot.Instance.m_DragSlot = this;
                UI_DragSlot.Instance.SetColor(1);
                UI_DragSlot.Instance.DragSetImage(m_SlotImage);
                UI_DragSlot.Instance.transform.position = PointerEventData.position;
            }
        }
        , Defines.UIEvent.BeginDrag);

        gameObject.BindEvent((PointerEventData) =>
        {
            if (m_Item != null)
            {
                //Debug.Log($"{m_Item.m_ItemName} �巡�� ��!");

                UI_DragSlot.Instance.transform.position = PointerEventData.position;
            }
        }
        , Defines.UIEvent.Drag);

        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} �巡�� ����!");

            // �κ��丮 �� �������� ������ ����� �Ǹ�
            if (!((UI_DragSlot.Instance.transform.localPosition.x > m_BaseRect.xMin
            && UI_DragSlot.Instance.transform.localPosition.x < m_BaseRect.xMax
            && UI_DragSlot.Instance.transform.localPosition.y > m_BaseRect.yMin
            && UI_DragSlot.Instance.transform.localPosition.y < m_BaseRect.yMax)
            || 
            (UI_DragSlot.Instance.transform.localPosition.x < m_QuickSlotBaseRect.transform.localPosition.x - m_QuickSlotBaseRect.rect.xMin - 560
            && UI_DragSlot.Instance.transform.localPosition.x > m_QuickSlotBaseRect.transform.localPosition.x - m_QuickSlotBaseRect.rect.xMax - 560
            && UI_DragSlot.Instance.transform.localPosition.y < m_QuickSlotBaseRect.transform.localPosition.y - m_QuickSlotBaseRect.rect.yMin
            && UI_DragSlot.Instance.transform.localPosition.y > m_QuickSlotBaseRect.transform.localPosition.y - m_QuickSlotBaseRect.rect.yMax)))
            {
                // �츮�� ������ �ý��� �ȳ��� ���� => �ı� �ý��� (�ν�Ʈ��ũ ���)
                if (UI_DragSlot.Instance.m_DragSlot != null)
                {
                    m_InputNumber.Call();
                }
            }
            else
            {
                UI_DragSlot.Instance.SetColor(0);
                UI_DragSlot.Instance.m_DragSlot = null;
            }
        }
        , Defines.UIEvent.EndDrag);

        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} ���!");

            if (UI_DragSlot.Instance.m_DragSlot != null)
            {
                ChangeSlot();
            }
        }
        , Defines.UIEvent.Drop);
    }

    // ���� ü����
    public void ChangeSlot()
    {
        Item tempItem = m_Item;
        int tempItemCount = m_ItemCount;

        AddItem(UI_DragSlot.Instance.m_DragSlot.m_Item, UI_DragSlot.Instance.m_DragSlot.m_ItemCount);

        if(tempItem != null)
            UI_DragSlot.Instance.m_DragSlot.AddItem(tempItem, tempItemCount);
        else
            UI_DragSlot.Instance.m_DragSlot.ClearSlot();
    }

    // ������ ���� ����
    public void SetSlotCount(int count)
    {
        m_ItemCount += count;
        m_SlotCount.text = m_ItemCount.ToString();

        if (m_ItemCount <= 0)
            ClearSlot();
    }

    // ���� �ʱ�ȭ
    void ClearSlot()
    {
        m_Item = null;
        m_ItemCount = 0;
        m_SlotImage.sprite = null;
        SetColor(0);

        m_SlotCount.text = null;
    }
}
