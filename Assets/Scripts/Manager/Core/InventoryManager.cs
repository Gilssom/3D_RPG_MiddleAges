using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletomManager<InventoryManager>
{
    protected InventoryManager() { }

    public static bool m_InventoryActivated = false;

    [SerializeField]
    private UI_Inven m_InvenBase;

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
        m_InvenBase = Inven.GetComponent<UI_Inven>();

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
}
