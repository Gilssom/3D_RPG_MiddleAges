using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    public int m_ComboCount { get; set; }                                                // ������ ���� �޺� ī��Ʈ
    public WeaponHandleData m_HandleData { get { return weaponHandleData; } }            // ���⸦ ����� �� ���� ��ǥ
    public RuntimeAnimatorController m_WeaponAnimator { get { return weaponAnimator; } } // �ش� ���� �ִϸ�����
    public string m_Name { get { return _name; } }
    public float m_AttackDamage { get { return attackDamage; } }
    public float m_AttackSpeed { get { return attackSpeed; } }
    public float m_AttackRange { get { return attackRange; } }
    public GameObject m_UltimateSkillObject { get { return UltiSkillObject; } }

    #region #���� ����
    [Header("Create ����"), Tooltip("�ش� ������ Local Position")]
    [SerializeField] protected WeaponHandleData weaponHandleData;

    [Header("Weapon ����")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    [Header("Skill ����")]
    [SerializeField] protected GameObject UltiSkillObject;
    #endregion

    public void SetWeaponData(string name, float attackDamage, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state);           // �⺻ ����
    public abstract void KickAttack(BaseState state);       // ��� ����
    public abstract void DashAttack(BaseState state);       // ��� ����
    public abstract void ChargingAttack(BaseState state);   // ���� ����
    public abstract void Skill(BaseState state);            // ��ų
    public abstract void UltimateSkill(BaseState state);    // �ñر�
}
