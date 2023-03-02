using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    public int m_ComboCount { get; set; }                                                // 무기의 현재 콤보 카운트
    public WeaponHandleData m_HandleData { get { return weaponHandleData; } }            // 무기를 쥐었을 때 로컬 좌표
    public RuntimeAnimatorController m_WeaponAnimator { get { return weaponAnimator; } } // 해당 무기 애니메이터
    public string m_Name { get { return _name; } }
    public float m_AttackDamage { get { return attackDamage; } }
    public float m_AttackSpeed { get { return attackSpeed; } }
    public float m_AttackRange { get { return attackRange; } }
    public GameObject m_UltimateSkillObject { get { return UltiSkillObject; } }

    #region #무기 정보
    [Header("Create 정보"), Tooltip("해당 무기의 Local Position")]
    [SerializeField] protected WeaponHandleData weaponHandleData;

    [Header("Weapon 정보")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    [Header("Skill 정보")]
    [SerializeField] protected GameObject UltiSkillObject;

    [Header("재공격 가능 시간")]
    [SerializeField] protected float m_CanReInputTime;

    [Header("강화 수치에 따른 무기 이펙트")]
    [SerializeField] protected EffectData m_Effect;
    #endregion

    public void SetWeaponData(string name, float attackDamage, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public void SetEffect()
    {
        if (BaseInfo.playerInfo.m_WeaponLevel < 10)
            return;

        if (BaseInfo.playerInfo.m_WeaponLevel > 10)
        {
            Transform[] childList = gameObject.GetComponentsInChildren<Transform>();

            if (childList != null)
            {
                for (int i = 0; i < childList.Length; i++)
                {
                    if (childList[i] != transform)
                    {
                        ResourcesManager.Instance.Destroy(childList[i].gameObject);
                    }
                }
            }
        }

        Instantiate(m_Effect.m_EffectList[BaseInfo.playerInfo.m_WeaponLevel - 10].m_EffectPrefab, this.transform);
    }

    public abstract void Attack(BaseState state);           // 기본 공격
    public abstract void KickAttack(BaseState state);       // 대시 공격
    public abstract void DashAttack(BaseState state);       // 대시 공격
    public abstract void ChargingAttack(BaseState state);   // 차지 공격
    public abstract void Skill(BaseState state);            // 스킬
    public abstract void UltimateSkill(BaseState state);    // 고대의 창
    public abstract void BladeSkill(BaseState state);       // 임페일 블레이드
    public abstract void DevilSlashSkill(BaseState state);       // 데빌 슬래쉬 
    public abstract void LightRefereeSkill(BaseState state);       // 빛의 심판
    public abstract void AngelSkill(BaseState state);       // 천사의 포옹
}
