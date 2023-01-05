using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private static InventoryManager instance;
    public GameObject m_NormalSword;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    void Start()
    {
        Init();
    }

    // ���� ���� �� ������ ��� ������ ���⸦ �о���� �ʱ�ȭ �Լ�
    private void Init()
    {
        GameObject weapon = Instantiate(m_NormalSword);
        PlayerInfo.Instance.playerInfo.m_WeaponManager.RegisterWeapon(weapon);
        PlayerInfo.Instance.playerInfo.m_WeaponManager.SetWeapon(weapon);
    }
}
