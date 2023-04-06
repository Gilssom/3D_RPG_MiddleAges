using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Info_Slot : UI_Base
{
    [SerializeField]
    protected Image m_ItemImage;
    [SerializeField]
    protected Item m_Item;
    [SerializeField]
    protected int m_EnforceLevel;

    public Defines.ItemType m_ItemEffectType;
    public Defines.ItemClass m_ItemEffectClass;

    public override void Init()
    {
        SetEvent();
    }

    private void OnEnable()
    {
        SetEnforceLevel();
        SetEnforceClass();
        SetEquirItem();
    }

    protected void SetEnforceLevel()
    {
        switch (m_ItemEffectType)
        {
            case Defines.ItemType.Helmat:
                m_EnforceLevel = BaseInfo.playerInfo.m_HelmatLevel;
                break;
            case Defines.ItemType.Shoulder:
                m_EnforceLevel = BaseInfo.playerInfo.m_ShoulderLevel;
                break;
            case Defines.ItemType.Top:
                m_EnforceLevel = BaseInfo.playerInfo.m_TopLevel;
                break;
            case Defines.ItemType.Bottom:
                m_EnforceLevel = BaseInfo.playerInfo.m_BottomLevel;
                break;
            case Defines.ItemType.Glove:
                m_EnforceLevel = BaseInfo.playerInfo.m_GloveLevel;
                break;
            case Defines.ItemType.Weapon:
                m_EnforceLevel = BaseInfo.playerInfo.m_WeaponLevel;
                break;
        }
    }

    protected void SetEnforceClass()
    {
        if (m_EnforceLevel >= 15)
            m_ItemEffectClass = Defines.ItemClass.Relics;
        else if (m_EnforceLevel >= 10)
            m_ItemEffectClass = Defines.ItemClass.Legandary;
        else if (m_EnforceLevel >= 5)
            m_ItemEffectClass = Defines.ItemClass.Epic;
        else
            m_ItemEffectClass = Defines.ItemClass.Rair;
    }

    protected void SetEquirItem()
    {
        m_Item = ResourcesManager.Instance.Load<Item>($"Data/ItemData/Equirment/{m_ItemEffectClass}/{m_ItemEffectClass} {m_ItemEffectType}");
        m_ItemImage.sprite = m_Item.m_ItemImage;
    }

    protected void SetEvent()
    {
        gameObject.BindEvent((PointerEventData) =>
        {
            if (m_Item != null)         
                InventoryManager.Instance.ShowToolTip(m_Item, GetComponent<RectTransform>(), m_EnforceLevel);        
        }
        , Defines.UIEvent.Enter);

        gameObject.BindEvent((PointerEventData) =>
        {
            InventoryManager.Instance.HideToolTip();
        }
        , Defines.UIEvent.Exit);
    }
}
