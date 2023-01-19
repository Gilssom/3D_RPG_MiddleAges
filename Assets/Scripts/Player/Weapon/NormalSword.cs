using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class NormalSword : BaseWeapon
{
    public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
    public readonly int IsChargeAnimation = Animator.StringToHash("isCharge");
    public readonly int IsKickAnimation = Animator.StringToHash("isKick");
    public readonly int IsUltiSkillAnimation = Animator.StringToHash("isUltiSkill");
    public readonly int AttackComboAnimation = Animator.StringToHash("AttackCombo");
    public readonly int AttackSpeedAnimation = Animator.StringToHash("AttackSpeed");

    private Coroutine m_CheckAttackReInputCor;

    public override void Attack(BaseState state)
    {
        m_ComboCount++;
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, m_AttackSpeed);
        BaseInfo.playerInfo.m_Anim.SetBool(IsAttackAnimation, true);
        BaseInfo.playerInfo.m_Anim.SetInteger(AttackComboAnimation, m_ComboCount);
        CheckAttackReInput(AttackState.m_CanReInputTime);
    }

    public override void ChargingAttack(BaseState state)
    {
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, m_AttackSpeed);
        BaseInfo.playerInfo.m_Anim.SetBool(IsChargeAnimation, true);
    }

    public override void KickAttack(BaseState state)
    {
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, m_AttackSpeed);
        BaseInfo.playerInfo.m_Anim.SetBool(IsKickAnimation, true);
    }

    public override void DashAttack(BaseState state)
    {

    }

    public override void Skill(BaseState state)
    {

    }

    public override void UltimateSkill(BaseState state)
    {
        Debug.Log("Ultimate Skill On");
        Camera.main.fieldOfView = 80;
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 0.5f);
        BaseInfo.playerInfo.m_Anim.SetBool(IsUltiSkillAnimation, true);
        ResourcesManager.Instance.Instantiate("Player/Ultimate_Effect");
    }

    public void CheckAttackReInput(float ReInputTime)
    {
        if (m_CheckAttackReInputCor != null)
            StopCoroutine(m_CheckAttackReInputCor);
        m_CheckAttackReInputCor = StartCoroutine(CheckAttackReInputCoroutine(ReInputTime));
    }

    private IEnumerator CheckAttackReInputCoroutine(float ReInputTime)
    {
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;

            if (curTime >= ReInputTime)
                break;

            yield return null;
        }

        m_ComboCount = 0;
        BaseInfo.playerInfo.m_Anim.SetInteger(AttackComboAnimation, 0);
    }
}
