using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletomManager<InventoryManager>
{
    protected InventoryManager() { }

    public GameObject[] m_Swords;

    void Start()
    {
        Init();
    }

    // ���� ���� �� ������ ��� ������ ���⸦ �о���� �ʱ�ȭ �Լ�
    private void Init()
    {
        //GameObject[] weapons = m_Swords;

        for (int i = 0; i < m_Swords.Length; i++)
        {
            GameObject weapon = Instantiate(m_Swords[i]);
            BaseInfo.playerInfo.m_WeaponManager.RegisterWeapon(weapon);
        }

        BaseInfo.playerInfo.m_WeaponManager.SetWeapon(null);
    }
}
