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
    #endregion

    public void SetWeaponData(string name, float attackDamage, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state);           // 기본 공격
    public abstract void KickAttack(BaseState state);       // 대시 공격
    public abstract void DashAttack(BaseState state);       // 대시 공격
    public abstract void ChargingAttack(BaseState state);   // 차지 공격
    public abstract void Skill(BaseState state);            // 스킬
    public abstract void UltimateSkill(BaseState state);    // 궁극기
}
