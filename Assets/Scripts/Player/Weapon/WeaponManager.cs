using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager
{
    public BaseWeapon m_Weapon { get; private set; } // 현재 무기 스크립트
    public Action<GameObject> m_UnRegisterWeapon { get; set; }

    private Transform m_HandPos;                                 // 무기를 쥐는 손의 위치
    private GameObject m_WeaponObject;                           // 현재 무기 오브젝트
    private List<GameObject> m_Weapons = new List<GameObject>(); // 현재 매니저에 등록된 무기 리스트

    public WeaponManager(Transform hand)
    {
        m_HandPos = hand;
    }

    // 무기 등록
    public void RegisterWeapon(GameObject weapon)
    {
        if(!m_Weapons.Contains(weapon))
        {
            BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
            weapon.transform.SetParent(m_HandPos);
            weapon.transform.localPosition = weaponInfo.m_HandleData.m_LocalPosition;
            weapon.transform.localEulerAngles = weaponInfo.m_HandleData.m_LocalRotation;
            weapon.transform.localScale = weaponInfo.m_HandleData.m_LocalScale;
            m_Weapons.Add(weapon);
            weapon.SetActive(false);
        }
    }

    // 무기 삭제
    public void UnRegisterWeapon(GameObject weapon)
    {
        if(m_Weapons.Contains(weapon))
        {
            m_Weapons.Remove(weapon);
            m_UnRegisterWeapon.Invoke(weapon);
        }
    }

    // 무기 변경
    public void SetWeapon(GameObject weapon)
    {
        if(m_Weapon == null)
        {
            Debug.Log("무기 처음 장착");
            m_WeaponObject = m_Weapons[0];
            m_Weapon = m_Weapons[0].GetComponent<BaseWeapon>();
            m_WeaponObject.SetActive(true);
            BaseInfo.playerInfo.m_Anim.runtimeAnimatorController = m_Weapon.m_WeaponAnimator;
            return;
        }

        for (int i = 0; i < m_Weapons.Count; i++)
        {
            Debug.Log(m_Weapons[i]);
            Debug.Log(m_Weapon.gameObject);
            Debug.Log(!m_Weapons[i].Equals(m_Weapon.gameObject));
            if (!m_Weapons[i].Equals(m_Weapon.gameObject))
            {
                m_WeaponObject = m_Weapons[i];
                m_Weapon.gameObject.SetActive(false);
                m_WeaponObject.SetActive(true);
                m_Weapon = m_Weapons[i].GetComponent<BaseWeapon>();
                m_Weapon.SetEffect();
                BaseInfo.playerInfo.m_Anim.runtimeAnimatorController = m_Weapon.m_WeaponAnimator;
                break;
            }
        }
    }
}
