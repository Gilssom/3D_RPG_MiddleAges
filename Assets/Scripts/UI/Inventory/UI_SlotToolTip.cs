using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UI_SlotToolTip : UI_Popup
{
    enum GameObjects
    {
        UI_ToolTipBase
    }

    enum Images
    {
        UI_ItemBack,
        UI_ItemImage
    }

    enum Texts
    {
        UI_ItemName,
        UI_ItemClass,
        UI_ItemType,
        UI_ItemInfo,
        UI_UsedInfo
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        GetObject((int)GameObjects.UI_ToolTipBase).SetActive(false);
    }

    public void ShowToolTip(Item item, Vector3 pos)
    {
        GetObject((int)GameObjects.UI_ToolTipBase).SetActive(true);

        pos += new Vector3(-(float)(GetObject((int)GameObjects.UI_ToolTipBase).GetComponent<RectTransform>().rect.width * 0.5),
            -(float)(GetObject((int)GameObjects.UI_ToolTipBase).GetComponent<RectTransform>().rect.height * 0.5), 0f);

        GetObject((int)GameObjects.UI_ToolTipBase).transform.position = pos;

        GetImage((int)Images.UI_ItemImage).sprite = item.m_ItemImage;

        SetDesc(item);
    }

    public void HideToolTip()
    {
        GetObject((int)GameObjects.UI_ToolTipBase).SetActive(false);
    }

    void SetDesc(Item item)
    {
        Color color;
        GetText((int)Texts.UI_ItemClass).text = item.m_ItemClassInfo;
        GetText((int)Texts.UI_ItemName).text = item.m_ItemName;
        GetText((int)Texts.UI_ItemType).text = item.m_ItemTypeInfo;
        GetText((int)Texts.UI_ItemInfo).text = item.m_ItemInfo;

        switch (item.m_ItemClass)
        {
            case Item.ItemClass.Normal:
                GetImage((int)Images.UI_ItemBack).color = new Color(0,0,0,0);
                ColorUtility.TryParseHtmlString("#BCBCBC", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Advanced:
                GetImage((int)Images.UI_ItemBack).color = new Color(150f/255f, 1, 0, 60f/255f);
                ColorUtility.TryParseHtmlString("#96FF00", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Rair:
                GetImage((int)Images.UI_ItemBack).color = new Color(0, 74f/255f, 1, 60f/255f);
                ColorUtility.TryParseHtmlString("#004AFF", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Epic:
                GetImage((int)Images.UI_ItemBack).color = new Color(231f/255f, 0, 1, 60f/255f);
                ColorUtility.TryParseHtmlString("#E700FF", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Legendary:
                GetImage((int)Images.UI_ItemBack).color = new Color(1, 129f/255f, 0, 60f/255f);
                ColorUtility.TryParseHtmlString("#FF8100", out color);
                SetColor(color);
                break;
        }

        if (item.m_ItemType == Item.ItemType.Equipment)
            GetText((int)Texts.UI_UsedInfo).text = "우클릭 - 장착";
        else if (item.m_ItemType == Item.ItemType.Used)
            GetText((int)Texts.UI_UsedInfo).text = "우클릭 - 사용";
        else
            GetText((int)Texts.UI_UsedInfo).text = "";
    }

    void SetColor(Color color)
    {
        GetText((int)Texts.UI_ItemClass).color = color;
        GetText((int)Texts.UI_ItemName).color = color;
    }
}
