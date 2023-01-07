using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletomManager<InventoryManager>
{
    protected InventoryManager() { }

    public GameObject m_NormalSword;

    void Start()
    {
        Init();
    }

    // ���� ���� �� ������ ��� ������ ���⸦ �о���� �ʱ�ȭ �Լ�
    private void Init()
    {
        GameObject weapon = Instantiate(m_NormalSword);
        Debug.Log(BaseInfo.playerInfo);
        BaseInfo.playerInfo.m_WeaponManager.RegisterWeapon(weapon);
        BaseInfo.playerInfo.m_WeaponManager.SetWeapon(weapon);
    }
}
