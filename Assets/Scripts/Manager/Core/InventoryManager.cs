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

    // 게임 시작 후 유저의 사용 가능한 무기를 읽어오는 초기화 함수
    private void Init()
    {
        GameObject weapon = Instantiate(m_NormalSword);
        Debug.Log(BaseInfo.playerInfo);
        BaseInfo.playerInfo.m_WeaponManager.RegisterWeapon(weapon);
        BaseInfo.playerInfo.m_WeaponManager.SetWeapon(weapon);
    }
}
