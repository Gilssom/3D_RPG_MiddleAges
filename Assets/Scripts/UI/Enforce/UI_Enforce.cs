using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class EnforceStatTotal : SingletomManager<EnforceStatTotal>
{
    [Header("머리 방어구 능력치 상승량")]
    public int AddHp_0;
    public int AddDF_0;
    [Header("어깨장식 능력치 상승량")]
    public int AddDF_1;
    public int AddCriChance_1;
    [Header("상의 능력치 상승량")]
    public int AddHp_2;
    [Header("하의 능력치 상승량")]
    public int AddDF_3;
    [Header("장갑 능력치 상승량")]
    public int AddCriChance_4;
    public float AddCriDamage_4;
    [Header("무기 능력치 상승량")]
    public int AddAttack_5;
}

public class UI_Enforce : UI_Popup
{
    public DataManager m_DataManager { get; private set; }

    enum GameObjects
    {
        EnforceInfo,
        UI_Info_GridArea,
        UI_Shop_BackGround,
        UI_CheckBase,
    }

    enum Buttons
    {
        UI_EnforceButton,
        UI_TestButton,
        UI_OkButton,
    }

    enum Images
    {
        SelectImage,
        IngreImage_1,
        IngreImage_2,
        IngreImage_3,
    }

    enum Texts
    {
        SelectName,
        SelectPercent,
        IngreCount_1,
        IngreCount_2,
        IngreCount_3,
        GoldCount,
        UI_SuccessText,
    }

    [SerializeField]
    private UI_Enforce_Slot[] m_Slots;
    private int[] m_NeedCount;
    private int m_NeedFragCount, m_NeedGold;
    private float m_Success;

    [SerializeField]
    private Item m_Item;
    [SerializeField]
    private Item[] m_NeedItem;

    public override void Init()
    {
        base.Init();

        m_DataManager = new DataManager();
        m_NeedItem = new Item[2];
        m_NeedCount = new int[2];

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.UI_Info_GridArea);
        m_Slots = gridPanel.GetComponentsInChildren<UI_Enforce_Slot>();

        m_DataManager.Init("EnforceData");
        m_DataManager.Init("EnforceStatData");

        for (int i = 0; i < m_Slots.Length; i++)
        {
            m_Slots[i].SetItemStat();
        }

        GetButton((int)Buttons.UI_EnforceButton).gameObject.BindEvent(EnforceButton);
        GetButton((int)Buttons.UI_OkButton).gameObject.BindEvent(EnforceCheckButton);
        GetButton((int)Buttons.UI_TestButton).gameObject.BindEvent(TestEnforceSuccess);
        GetObject((int)GameObjects.EnforceInfo).SetActive(false);
        GetObject((int)GameObjects.UI_Shop_BackGround).SetActive(false);
        GetObject((int)GameObjects.UI_CheckBase).SetActive(false);
    }

    public void SetEnforceStat(int enforceLevel, Defines.ItemType itemType)
    {
        Dictionary<int, Data.EnforceStatData> dict = m_DataManager.EnforceStatDict;
        Data.EnforceStatData stat = dict[enforceLevel];
        PlayerInfo playerStat = BaseInfo.playerInfo;

        switch (itemType)
        {
            case Defines.ItemType.Helmat:
                playerStat.m_Plus_MaxHp += stat.minMaxHp;
                playerStat.m_Plus_Defense += stat.minDefense;

                BaseInfo.playerInfo.MaxHp += stat.minMaxHp;
                BaseInfo.playerInfo.Hp += stat.minMaxHp;
                BaseInfo.playerInfo .Defense+= stat.minDefense;

                EnforceStatTotal.Instance.AddHp_0 += stat.minMaxHp;
                EnforceStatTotal.Instance.AddDF_0 += stat.minDefense;
                break;
            case Defines.ItemType.Shoulder:
                playerStat.m_Plus_Defense += stat.minDefense;
                playerStat.m_Plus_CriChance += stat.criChance;

                BaseInfo.playerInfo.Defense += stat.minDefense;
                BaseInfo.playerInfo.CriticalChance += stat.criChance;

                EnforceStatTotal.Instance.AddDF_1 += stat.minDefense;
                EnforceStatTotal.Instance.AddCriChance_1 += stat.criChance;
                break;
            case Defines.ItemType.Top:
                playerStat.m_Plus_MaxHp += stat.maxMaxHp;

                BaseInfo.playerInfo.MaxHp += stat.maxMaxHp;
                BaseInfo.playerInfo.Hp += stat.maxMaxHp;

                EnforceStatTotal.Instance.AddHp_2 += stat.maxMaxHp;
                break;
            case Defines.ItemType.Bottom:
                playerStat.m_Plus_Defense += stat.maxDefense;

                BaseInfo.playerInfo.Defense += stat.maxDefense;

                EnforceStatTotal.Instance.AddDF_3 += stat.maxDefense;
                break;
            case Defines.ItemType.Glove:
                playerStat.m_Plus_CriChance += stat.criChance;
                playerStat.m_Plus_CriDamage += stat.criDamage;

                BaseInfo.playerInfo.CriticalChance += stat.criChance;
                BaseInfo.playerInfo.CriticalDamage += stat.criDamage;

                EnforceStatTotal.Instance.AddCriChance_4 += stat.criChance;
                EnforceStatTotal.Instance.AddCriDamage_4 += stat.criDamage;
                break;
            case Defines.ItemType.Weapon:
                playerStat.m_Plus_Attack += stat.attack;

                BaseInfo.playerInfo.Attack += stat.attack;

                EnforceStatTotal.Instance.AddAttack_5 += stat.attack;
                break;
        }
    }

    public void OpenEnforce()
    {
        GetObject((int)GameObjects.EnforceInfo).SetActive(true);
    }

    public void CloseEnforce()
    {
        GetObject((int)GameObjects.UI_Shop_BackGround).SetActive(false);
        GetObject((int)GameObjects.EnforceInfo).SetActive(false);
    }

    void EnforceButton(PointerEventData data)
    {
        GetObject((int)GameObjects.UI_CheckBase).SetActive(true);

        if (CheckItemCount())
        {
            UseItem();
            float Chance = Random.Range(0, 100);
            if (Chance < m_Success)
            {
                GetText((int)Texts.UI_SuccessText).text = "강화 성공!";
                SoundManager.Instance.Play("UI/Enforce Success");
                EnforceSuccess();
            }
            else
            {
                GetText((int)Texts.UI_SuccessText).text = "강화 실패!";
                SoundManager.Instance.Play("UI/Enforce Fail");
            }
        }
        else
        {
            GetText((int)Texts.UI_SuccessText).text = "재료가 부족합니다.";
            SoundManager.Instance.Play("UI/Fail");
        }
    }

    private bool CheckItemCount()
    {
        for (int i = 0; i < m_NeedItem.Length; i++)
        {
            if (InventoryManager.Instance.m_InvenBase.SearchSlotItem(m_NeedItem[i].m_ItemName)
                < m_NeedCount[i] 
                || BaseInfo.playerInfo.Gold < m_NeedGold
                || BaseInfo.playerInfo.Fragments < m_NeedFragCount)
            {
                Debug.Log("재료가 충분하지 않습니다.");
                return false;
            }
        }

        return true;
    }

    void EnforceSuccess()
    {
        switch (m_Item.m_ItemPart)
        {
            case Item.ItemParts.Helmat:
                BaseInfo.playerInfo.m_HelmatLevel++;
                break;
            case Item.ItemParts.Shoulder:
                BaseInfo.playerInfo.m_ShoulderLevel++;
                break;
            case Item.ItemParts.Top:
                BaseInfo.playerInfo.m_TopLevel++;
                break;
            case Item.ItemParts.Bottom:
                BaseInfo.playerInfo.m_BottomLevel++;
                break;
            case Item.ItemParts.Glove:
                BaseInfo.playerInfo.m_GloveLevel++;
                break;
            case Item.ItemParts.Weapon:
                BaseInfo.playerInfo.m_WeaponLevel++;
                break;
        }

        for (int i = 0; i < m_Slots.Length; i++)
        {
            m_Slots[i].SetRefresh();
        }

        SetPlayerAddStat(m_Item.m_ItemPart);
    }

    void UseItem()
    {
        for (int i = 0; i < m_NeedItem.Length; i++)
        {
            InventoryManager.Instance.m_InvenBase.SetItemCount(m_NeedItem[i].m_ItemName, m_NeedCount[i]);
        }
        BaseInfo.playerInfo.Gold -= m_NeedGold;
        BaseInfo.playerInfo.Fragments -= m_NeedFragCount;
    }

    void EnforceCheckButton(PointerEventData data)
    {
        GetObject((int)GameObjects.UI_CheckBase).SetActive(false);
        GetObject((int)GameObjects.UI_Shop_BackGround).SetActive(false);
    }

    public void SelectItem(Item item, Color color, int Level)
    {
        SetData(item, Level);

        GetObject((int)GameObjects.UI_Shop_BackGround).SetActive(true);

        SetDataInfo(item);

        GetImage((int)Images.SelectImage).sprite = item.m_ItemImage;

        GetText((int)Texts.SelectName).text = $"{item.m_ItemName} +{Level}";
        GetText((int)Texts.SelectName).color = color;

        m_Item = item;
    }

    void SetData(Item item, int itemLevel)
    {
        Dictionary<int, Data.EnforceData> dict = m_DataManager.EnforceDict;
        Data.EnforceData stat = dict[itemLevel];
       
        if (item.m_ItemPart == Item.ItemParts.Weapon)
        {
            m_NeedCount[0] = (int)(stat.needMain * 1.3f);
            m_NeedCount[1] = (int)(stat.needSub * 1.3f);
            m_NeedFragCount = (int)(stat.needFragments * 1.3f);
            m_NeedGold = (int)(stat.needGold * 1.3f);
        }
        else
        {
            m_NeedCount[0] = stat.needMain;
            m_NeedCount[1] = stat.needSub;
            m_NeedFragCount = stat.needFragments;
            m_NeedGold = stat.needGold;
        }

        m_Success = stat.success;
    }

    void SetDataInfo(Item item)
    {
        GetText((int)Texts.IngreCount_1).text = $"{m_NeedCount[0]}개 필요";
        GetText((int)Texts.IngreCount_2).text = $"{m_NeedCount[1]}개 필요";
        GetText((int)Texts.IngreCount_3).text = $"{m_NeedFragCount}개 필요";
        GetText((int)Texts.GoldCount).text = m_NeedGold.ToString();
        GetText((int)Texts.SelectPercent).text = $"성공확률 {m_Success}%";

        switch (item.m_ItemClass)
        {
            case Item.ItemClass.Rair:
                NeedItemCheck(1);
                break;
            case Item.ItemClass.Epic:
                NeedItemCheck(2);
                break;
            case Item.ItemClass.Legendary:
                NeedItemCheck(3);
                break;
        }
    }

    void NeedItemCheck(int tier)
    {
        m_NeedItem[0] = ResourcesManager.Instance.Load<Item>($"Data/ItemData/Ingrident/Enforce/Main Tier {tier}");
        m_NeedItem[1] = ResourcesManager.Instance.Load<Item>($"Data/ItemData/Ingrident/Enforce/Sub Tier {tier}");

        GetImage((int)Images.IngreImage_1).sprite = m_NeedItem[0].m_ItemImage;
        GetImage((int)Images.IngreImage_2).sprite = m_NeedItem[1].m_ItemImage;
    }

    void SetPlayerAddStat(Item.ItemParts item)
    {
        for (int i = 0; i < m_Slots.Length; i++)
        {
            m_Slots[i].SetItemStat(item);
        }
    }

    void TestEnforceSuccess(PointerEventData data)
    {
        SoundManager.Instance.Play("UI/Enforce Success");

        switch (m_Item.m_ItemPart)
        {
            case Item.ItemParts.Helmat:
                BaseInfo.playerInfo.m_HelmatLevel++;
                break;
            case Item.ItemParts.Shoulder:
                BaseInfo.playerInfo.m_ShoulderLevel++;
                break;
            case Item.ItemParts.Top:
                BaseInfo.playerInfo.m_TopLevel++;
                break;
            case Item.ItemParts.Bottom:
                BaseInfo.playerInfo.m_BottomLevel++;
                break;
            case Item.ItemParts.Glove:
                BaseInfo.playerInfo.m_GloveLevel++;
                break;
            case Item.ItemParts.Weapon:
                BaseInfo.playerInfo.m_WeaponLevel++;
                BaseInfo.playerInfo.m_WeaponManager.m_Weapon.SetEffect();
                Debug.Log(BaseInfo.playerInfo.m_WeaponManager);
                break;
        }

        for (int i = 0; i < m_Slots.Length; i++)
        {
            m_Slots[i].SetRefresh();
        }

        SetPlayerAddStat(m_Item.m_ItemPart);
    }
}
