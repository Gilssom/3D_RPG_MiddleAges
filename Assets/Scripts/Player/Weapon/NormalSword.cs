using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class NormalSword : BaseWeapon
{
    public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
    public readonly int IsChargeAnimation = Animator.StringToHash("isCharge");
    public readonly int IsKickAnimation = Animator.StringToHash("isKick");
    public readonly int AttackComboAnimation = Animator.StringToHash("AttackCombo");
    public readonly int AttackSpeedAnimation = Animator.StringToHash("AttackSpeed");

    private Coroutine m_CheckAttackReInputCor;

    public override void Attack(BaseState state)
    {
        m_ComboCount++;
        PlayerInfo.Instance.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, m_AttackSpeed);
        PlayerInfo.Instance.playerInfo.m_Anim.SetBool(IsAttackAnimation, true);
        PlayerInfo.Instance.playerInfo.m_Anim.SetInteger(AttackComboAnimation, m_ComboCount);
        CheckAttackReInput(AttackState.m_CanReInputTime);
    }

    public override void ChargingAttack(BaseState state)
    {
        PlayerInfo.Instance.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, m_AttackSpeed);
        PlayerInfo.Instance.playerInfo.m_Anim.SetBool(IsChargeAnimation, true);
    }

    public override void KickAttack(BaseState state)
    {
        PlayerInfo.Instance.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 1);
        PlayerInfo.Instance.playerInfo.m_Anim.SetBool(IsKickAnimation, true);
    }

    public override void DashAttack(BaseState state)
    {

    }

    public override void Skill(BaseState state)
    {

    }

    public override void UltimateSkill(BaseState state)
    {

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
        PlayerInfo.Instance.playerInfo.m_Anim.SetInteger(AttackComboAnimation, 0);
    }
}
