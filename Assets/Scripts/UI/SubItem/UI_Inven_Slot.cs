using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Slot : UI_Base
{
    public Vector3 m_OriginPos;

    public Image m_SlotImage;
    public Text m_SlotCount;
    public Item m_Item;
    public int m_ItemCount;

    public override void Init() { }

    public void SetPos()
    {
        m_OriginPos = transform.position;
        SetEvent();
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

    void SetEvent()
    {
        // ������ Enter �̺�Ʈ
        gameObject.BindEvent((PointerEventData) => 
        { 
            //Debug.Log($"{m_Item.m_ItemName} ����!"); 
        }
        , Defines.UIEvent.Enter);

        // ������ ���� Ŭ�� �̺�Ʈ
        gameObject.BindEvent((PointerEventData) => 
        {
            //Debug.Log($"{m_Item.m_ItemName} Ŭ��!");

            if(m_Item != null)
            {
                if(m_Item.m_ItemType == Item.ItemType.Equipment)
                {
                    // ������ ����
                }
                else if(m_Item.m_ItemType == Item.ItemType.Used)
                {
                    // ������ �Ҹ�
                    Debug.Log($"{m_Item.m_ItemName}�� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
                else
                {
                    // ��Ÿ ������
                }
            }
        }
        );

        // ������ �巡�� �̺�Ʈ
        gameObject.BindEvent((PointerEventData) => 
        {
            if(m_Item != null)
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

            UI_DragSlot.Instance.SetColor(0);
            UI_DragSlot.Instance.m_DragSlot = null;
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

    public void ChangeSlot()
    {
        Item tempItem = m_Item;
        int tempItemCount = m_ItemCount;

        //Debug.Log($"���� ü���� / ���� ������ {tempItem} / ���� ������ ���� {tempItemCount}");

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
