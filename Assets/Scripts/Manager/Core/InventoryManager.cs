using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletomManager<InventoryManager>
{
    protected InventoryManager() { }

    public static bool m_InventoryActivated = false;
    public static bool m_PlayerInfoActivated = false;
    public static bool m_ShopActivated = false;
    public static bool m_EnforceActivated = false;
    public static bool m_SkillActivated = false;

    [SerializeField]
    public UI_Inven m_InvenBase;
    public UI_InputNumber m_InputNumber;
    public UI_SlotToolTip m_ToopTip;
    public RectTransform m_QuickSlotBaseRect;
    public UI_QuickSlotCtrl m_QuickSlot;
    public UI_PlayerInfo m_PlayerInfo;
    public UI_Shop m_PotionShop;
    public UI_Shop m_EnforceShop;
    public UI_Enforce m_EnforceSystem;
    public UI_Skill m_SkillSystem;
    public SkillKeyMap m_SkillQuickSlot;

    public GameObject[] m_Swords;

    void Start()
    {
        Init();
    }

    // 게임 시작 후 유저의 사용 가능한 무기를 읽어오는 초기화 함수
    private void Init()
    {
        //GameObject[] weapons = m_Swords;
        GameObject Inven = ResourcesManager.Instance.Instantiate("UI/Popup/UI_Inven", gameObject.transform);
        GameObject PotionShop = ResourcesManager.Instance.Instantiate("UI/Popup/UI_PotionShop", gameObject.transform);
        GameObject EnforceShop = ResourcesManager.Instance.Instantiate("UI/Popup/UI_EnforceShop", gameObject.transform);
        GameObject PlayerInfo = ResourcesManager.Instance.Instantiate("UI/Popup/UI_PlayerInfo", gameObject.transform);
        GameObject EnforceSystem = ResourcesManager.Instance.Instantiate("UI/Popup/UI_Enforce", gameObject.transform);
        GameObject SkillSystem = ResourcesManager.Instance.Instantiate("UI/Popup/UI_Skill", gameObject.transform);
        GameObject InputField = ResourcesManager.Instance.Instantiate("UI/Popup/UI_ThrowItem", gameObject.transform);
        GameObject ToolTip = ResourcesManager.Instance.Instantiate("UI/Popup/UI_ToolTip", gameObject.transform);

        m_QuickSlot = UIManager.Instance.ShowSceneUI<UI_QuickSlotCtrl>();
        m_SkillQuickSlot = UIManager.Instance.ShowSceneUI<SkillKeyMap>();
        m_InvenBase = Inven.GetComponent<UI_Inven>();
        m_InputNumber = InputField.GetComponent<UI_InputNumber>();
        m_ToopTip = ToolTip.GetComponent<UI_SlotToolTip>();
        m_PlayerInfo = PlayerInfo.GetComponent<UI_PlayerInfo>();
        m_PotionShop = PotionShop.GetComponent<UI_Shop>();
        m_EnforceShop = EnforceShop.GetComponent<UI_Shop>();
        m_EnforceSystem = EnforceSystem.GetComponent<UI_Enforce>();
        m_SkillSystem = SkillSystem.GetComponent<UI_Skill>();

        for (int i = 0; i < m_Swords.Length; i++)
        {
            GameObject weapon = Instantiate(m_Swords[i]);
            BaseInfo.playerInfo.m_WeaponManager.RegisterWeapon(weapon);
        }

        BaseInfo.playerInfo.m_WeaponManager.SetWeapon(null);
    }

    public void TryOpenInventory()
    {
        m_InventoryActivated = !m_InventoryActivated;

        if (m_InventoryActivated)
        {
            m_InvenBase.OpenInventory();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            m_InvenBase.CloseInventory();
            HideToolTip();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TryOpenPlayerInfo()
    {
        m_PlayerInfoActivated = !m_PlayerInfoActivated;

        if (m_PlayerInfoActivated)
        {
            m_PlayerInfo.OpenPlayerInfo();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            m_PlayerInfo.ClosePlayerInfo();
            HideToolTip();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TryOpenPotionShop()
    {
        m_ShopActivated = !m_ShopActivated;

        if (m_ShopActivated)
        {
            m_InvenBase.OpenInventory();
            m_PotionShop.OpenPotionShop();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            m_InvenBase.CloseInventory();
            m_PotionShop.ClosePotionShop();
            HideToolTip();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TryOpenEnforceShop()
    {
        m_ShopActivated = !m_ShopActivated;

        if (m_ShopActivated)
        {
            m_InvenBase.OpenInventory();
            m_EnforceShop.OpenPotionShop();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            m_InvenBase.CloseInventory();
            m_EnforceShop.ClosePotionShop();
            HideToolTip();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TryOpenEnforceSystem()
    {
        m_EnforceActivated = !m_EnforceActivated;

        if (m_EnforceActivated)
        {
            m_EnforceSystem.OpenEnforce();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            m_EnforceSystem.CloseEnforce();
            HideToolTip();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TryOpenSkillSystem()
    {
        m_SkillActivated = !m_SkillActivated;

        if (m_SkillActivated)
        {
            m_SkillSystem.OpenEnforce();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            m_SkillSystem.CloseEnforce();
            HideToolTip();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AcquireItem(Item item, int count = 1)
    {
        m_InvenBase.AcquireItem(item, count);
    }

    public void CtrlInputBase(bool OnOff)
    {
        if (!m_InventoryActivated)
            return;

        m_InputNumber.Activited(OnOff);
    }

    public void ShowToolTip(Item item, RectTransform pos, int itemLevel)
    {
        m_ToopTip.ShowToolTip(item, pos, itemLevel);
    }

    public void HideToolTip()
    {
        m_ToopTip.HideToolTip();
    }
}
