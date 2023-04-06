using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Slot : UI_Base
{
    [SerializeField]
    private Item m_Item;
    [SerializeField]
    private Text m_ItemName, m_ItemAddPrice, m_ItemAddLevel;
    [SerializeField]
    private Image m_ItemImage;
    private int m_ItemPrice;
    private int m_ItemLevel;

    public override void Init()
    {
        SetEvent();
    }

    public void SetData(string itemName, int itemPrice, int itemLevel, Item item)
    {
        m_ItemName.text = itemName;
        m_ItemAddPrice.text = itemPrice.ToString();
        m_ItemAddLevel.text = $"Lv. {itemLevel}";
        m_ItemImage.sprite = item.m_ItemImage;
        m_Item = item;
        m_ItemPrice = itemPrice;
        m_ItemLevel = itemLevel;
    }

    private void OnEnable()
    {
        if (BaseInfo.playerInfo.Level < m_ItemLevel)
        {
            GetComponent<Button>().interactable = false;
            m_ItemAddLevel.color = Color.red;
        }
        else
        {
            GetComponent<Button>().interactable = true;
            m_ItemAddLevel.color = Color.white;
        }
    }

    void SetEvent()
    {
        gameObject.BindEvent((PointerEventData) =>
        {
            if (m_Item != null)
                InventoryManager.Instance.ShowToolTip(m_Item, GetComponent<RectTransform>(), 0);
        }
        , Defines.UIEvent.Enter);

        gameObject.BindEvent((PointerEventData) =>
        {
            InventoryManager.Instance.HideToolTip();
        }
        , Defines.UIEvent.Exit);
    }

    public void BuyItemEvent()
    {
        if (m_Item != null)
        {
            if (BaseInfo.playerInfo.Gold < m_ItemPrice)
            {
                Debug.Log("돈이 부족합니다.");
                SoundManager.Instance.Play("UI/Fail");
                return;
            }

            BaseInfo.playerInfo.Gold -= m_ItemPrice;
            SoundManager.Instance.Play("UI/Success");
            InventoryManager.Instance.AcquireItem(m_Item);
        }
    }
}
