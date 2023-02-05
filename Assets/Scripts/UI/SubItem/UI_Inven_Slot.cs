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

    // 슬롯 아이템 이미지 투명도
    void SetColor(float alpha)
    {
        Color color = m_SlotImage.color;
        color.a = alpha;
        m_SlotImage.color = color;
    }

    // 아이템 추가
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
        // 아이템 Enter 이벤트
        gameObject.BindEvent((PointerEventData) => 
        { 
            //Debug.Log($"{m_Item.m_ItemName} 정보!"); 
        }
        , Defines.UIEvent.Enter);

        // 아이템 슬롯 클릭 이벤트
        gameObject.BindEvent((PointerEventData) => 
        {
            //Debug.Log($"{m_Item.m_ItemName} 클릭!");

            if(m_Item != null)
            {
                if(m_Item.m_ItemType == Item.ItemType.Equipment)
                {
                    // 아이템 장착
                }
                else if(m_Item.m_ItemType == Item.ItemType.Used)
                {
                    // 아이템 소모
                    Debug.Log($"{m_Item.m_ItemName}을 사용했습니다.");
                    SetSlotCount(-1);
                }
                else
                {
                    // 기타 아이템
                }
            }
        }
        );

        // 아이템 드래그 이벤트
        gameObject.BindEvent((PointerEventData) => 
        {
            if(m_Item != null)
            {
                //Debug.Log($"{m_Item.m_ItemName} 드래그 시작!");

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
                //Debug.Log($"{m_Item.m_ItemName} 드래그 중!");

                UI_DragSlot.Instance.transform.position = PointerEventData.position;
            }
        }
        , Defines.UIEvent.Drag);

        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} 드래그 종료!");

            UI_DragSlot.Instance.SetColor(0);
            UI_DragSlot.Instance.m_DragSlot = null;
        }
        , Defines.UIEvent.EndDrag);

        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} 드랍!");

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

        //Debug.Log($"슬롯 체인지 / 이전 아이템 {tempItem} / 이전 아이템 갯수 {tempItemCount}");

        AddItem(UI_DragSlot.Instance.m_DragSlot.m_Item, UI_DragSlot.Instance.m_DragSlot.m_ItemCount);

        if(tempItem != null)
            UI_DragSlot.Instance.m_DragSlot.AddItem(tempItem, tempItemCount);
        else
            UI_DragSlot.Instance.m_DragSlot.ClearSlot();
    }

    // 아이템 갯수 설정
    public void SetSlotCount(int count)
    {
        m_ItemCount += count;
        m_SlotCount.text = m_ItemCount.ToString();

        if (m_ItemCount <= 0)
            ClearSlot();
    }

    // 슬롯 초기화
    void ClearSlot()
    {
        m_Item = null;
        m_ItemCount = 0;
        m_SlotImage.sprite = null;
        SetColor(0);

        m_SlotCount.text = null;
    }
}
