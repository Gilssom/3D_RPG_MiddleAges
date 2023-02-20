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
        UI_UsedInfo,
        UI_ItemLevel,
    }

    private RectTransform m_Rect;
    private CanvasScaler m_CanvasScaler;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        GetObject((int)GameObjects.UI_ToolTipBase).SetActive(false);

        m_Rect = GetObject((int)GameObjects.UI_ToolTipBase).GetComponent<RectTransform>();
        m_Rect.pivot = new Vector2(0f, 1f);
        m_CanvasScaler = GetComponentInParent<CanvasScaler>();
    }

    public void ShowToolTip(Item item, RectTransform pos, int itemLevel)
    {
        GetObject((int)GameObjects.UI_ToolTipBase).SetActive(true);

        SetRectPos(pos);

        GetImage((int)Images.UI_ItemImage).sprite = item.m_ItemImage;

        SetDesc(item, itemLevel);
    }

    void SetRectPos(RectTransform slotRect)
    {
        // ĵ���� �����Ϸ��� ���� �ػ� ����
        float wRatio = Screen.width / m_CanvasScaler.referenceResolution.x;
        float hRatio = Screen.height / m_CanvasScaler.referenceResolution.y;
        float ratio =
            wRatio * (1f - m_CanvasScaler.matchWidthOrHeight) +
            hRatio * (m_CanvasScaler.matchWidthOrHeight);

        float slotWidth = slotRect.rect.width * ratio;
        float slotHeight = slotRect.rect.height * ratio;

        // ���� �ʱ� ��ġ(���� ���ϴ�) ����
        m_Rect.position = slotRect.position + new Vector3(slotWidth - 72.85f, -slotHeight + 72.85f);
        Vector2 pos = m_Rect.position;

        // ������ ũ��
        float width = m_Rect.rect.width * ratio;
        float height = m_Rect.rect.height * ratio;

        // ����, �ϴ��� �߷ȴ��� ����
        bool rightTruncated = pos.x + width > Screen.width;
        bool bottomTruncated = pos.y - height < 0f;

        ref bool R = ref rightTruncated;
        ref bool B = ref bottomTruncated;

        // �����ʸ� �߸� => ������ Left Bottom �������� ǥ��
        if (R && !B)
        {
            m_Rect.position = new Vector2(pos.x - width - slotWidth + 72.85f, pos.y);
        }
        // �Ʒ��ʸ� �߸� => ������ Right Top �������� ǥ��
        else if (!R && B)
        {
            m_Rect.position = new Vector2(pos.x, pos.y + height + slotHeight - 72.85f);
        }
        // ��� �߸� => ������ Left Top �������� ǥ��
        else if (R && B)
        {
            m_Rect.position = new Vector2(pos.x - width - slotWidth + 72.85f, pos.y + height + slotHeight - 72.85f);
        }
        // �߸��� ���� => ������ Right Bottom �������� ǥ��
        // Do Nothing
    }

    public void HideToolTip()
    {
        GetObject((int)GameObjects.UI_ToolTipBase).SetActive(false);
    }

    void SetDesc(Item item, int itemLevel)
    {
        Color color;
        GetText((int)Texts.UI_ItemClass).text = item.m_ItemClassInfo;
        GetText((int)Texts.UI_ItemName).text = item.m_ItemName;
        GetText((int)Texts.UI_ItemType).text = item.m_ItemTypeInfo;
        GetText((int)Texts.UI_ItemInfo).text = item.m_ItemInfo;
        GetText((int)Texts.UI_ItemLevel).text = $"+{itemLevel}";

        switch (item.m_ItemClass)
        {
            case Item.ItemClass.Normal:
                GetImage((int)Images.UI_ItemBack).color = new Color(0, 0, 0, 0);
                ColorUtility.TryParseHtmlString("#BCBCBC", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Advanced:
                GetImage((int)Images.UI_ItemBack).color = new Color(150f / 255f, 1, 0, 60f / 255f);
                ColorUtility.TryParseHtmlString("#96FF00", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Rair:
                GetImage((int)Images.UI_ItemBack).color = new Color(0, 74f / 255f, 1, 60f / 255f);
                ColorUtility.TryParseHtmlString("#004AFF", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Epic:
                GetImage((int)Images.UI_ItemBack).color = new Color(231f / 255f, 0, 1, 60f / 255f);
                ColorUtility.TryParseHtmlString("#E700FF", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Legendary:
                GetImage((int)Images.UI_ItemBack).color = new Color(1, 129f / 255f, 0, 60f / 255f);
                ColorUtility.TryParseHtmlString("#FF8100", out color);
                SetColor(color);
                break;
            case Item.ItemClass.Relics:
                GetImage((int)Images.UI_ItemBack).color = new Color(1, 22f / 255f, 0, 60f / 255f);
                ColorUtility.TryParseHtmlString("#FF1600", out color);
                SetColor(color);
                break;
        }

        if (item.m_ItemType == Item.ItemType.Equipment)
            GetText((int)Texts.UI_UsedInfo).text = "��Ŭ�� - ����";
        else if (item.m_ItemType == Item.ItemType.Used)
            GetText((int)Texts.UI_UsedInfo).text = "��Ŭ�� - ���";
        else
            GetText((int)Texts.UI_UsedInfo).text = "";

        if (itemLevel == 0)
            GetText((int)Texts.UI_ItemLevel).text = "";
    }

    void SetColor(Color color)
    {
        GetText((int)Texts.UI_ItemClass).color = color;
        GetText((int)Texts.UI_ItemName).color = color;
    }
}
