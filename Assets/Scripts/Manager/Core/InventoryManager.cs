using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletomManager<InventoryManager>
{
    protected InventoryManager() { }

    public static bool m_InventoryActivated = false;

    [SerializeField]
    private UI_Inven m_InvenBase;
    public UI_InputNumber m_InputNumber;

    public GameObject[] m_Swords;

    void Start()
    {
        Init();
    }

    // ���� ���� �� ������ ��� ������ ���⸦ �о���� �ʱ�ȭ �Լ�
    private void Init()
    {
        //GameObject[] weapons = m_Swords;
        GameObject Inven = ResourcesManager.Instance.Instantiate("UI/Popup/UI_Inven", gameObject.transform);
        GameObject InputField = ResourcesManager.Instance.Instantiate("UI/Popup/UI_ThrowItem", gameObject.transform);
        m_InvenBase = Inven.GetComponent<UI_Inven>();
        m_InputNumber = InputField.GetComponent<UI_InputNumber>();

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
            m_InvenBase.OpenInventory();
        else
            m_InvenBase.CloseInventory();
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
}
