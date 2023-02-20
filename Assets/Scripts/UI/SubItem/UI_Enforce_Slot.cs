using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Enforce_Slot : UI_Info_Slot
{
    enum Buttons
    {
        UI_Enforce_Slot,
    }

    [SerializeField]
    private Text m_ItemName, m_ItemLevel;
    [SerializeField]
    private UI_Enforce m_EnforceUI;

    public override void Init()
    {
        SetEvent();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.UI_Enforce_Slot).gameObject.BindEvent(SelectItem);
    }

    private void OnEnable()
    {
        SetRefresh();
    }

    public void SetRefresh()
    {
        SetEnforceLevel();
        SetEnforceClass();
        SetEquirItem();

        m_ItemLevel.text = $"+{m_EnforceLevel}";

        if (m_Item != null)
        {
            m_ItemName.text = m_Item.m_ItemName;
            SetInfoColor();
        }
    }

    void SetInfoColor()
    {
        Color color;

        switch (m_Item.m_ItemClass)
        {
            case Item.ItemClass.Normal:
                ColorUtility.TryParseHtmlString("#BCBCBC", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Advanced:
                ColorUtility.TryParseHtmlString("#96FF00", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Rair:
                ColorUtility.TryParseHtmlString("#004AFF", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Epic:
                ColorUtility.TryParseHtmlString("#E700FF", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Legendary:
                ColorUtility.TryParseHtmlString("#FF8100", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Relics:
                ColorUtility.TryParseHtmlString("#FF1600", out color);
                SetColor(color);
                break;
        }
    }

    void SetColor(Color color)
    {
        m_ItemName.color = color;
    }

    void SelectItem(PointerEventData data)
    {
        m_EnforceUI.SelectItem(m_Item, m_ItemName.color, m_EnforceLevel);
    }
}
