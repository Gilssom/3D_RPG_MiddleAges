using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class SwordInfo : BaseWeapon
{
    public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
    public readonly int IsChargeAnimation = Animator.StringToHash("isCharge");
    public readonly int IsKickAnimation = Animator.StringToHash("isKick");
    public readonly int IsBladeSkillAnimation = Animator.StringToHash("isBladeSkill");
    public readonly int IsUltiSkillAnimation = Animator.StringToHash("isUltiSkill");
    public readonly int IsBuffSkillAnimation = Animator.StringToHash("isBuffSkill");
    public readonly int AttackComboAnimation = Animator.StringToHash("AttackCombo");
    public readonly int AttackSpeedAnimation = Animator.StringToHash("AttackSpeed");

    private Coroutine m_CheckAttackReInputCor;

    public override void Attack(BaseState state)
    {
        m_ComboCount++;
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, m_AttackSpeed);
        BaseInfo.playerInfo.m_Anim.SetBool(IsAttackAnimation, true);
        BaseInfo.playerInfo.m_Anim.SetInteger(AttackComboAnimation, m_ComboCount);
        CheckAttackReInput(m_CanReInputTime);
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
        SoundManager.Instance.Play("Effect/Ultimate Skill Voice");
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 0.5f);
        BaseInfo.playerInfo.m_Anim.SetTrigger(IsUltiSkillAnimation);
        ResourcesManager.Instance.Instantiate("Player/Effect/Skill/Ultimate_Effect");
    }

    public override void BladeSkill(BaseState state)
    {
        Debug.Log("Impale Blade Skill On");
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 0.5f);
        BaseInfo.playerInfo.m_Anim.SetTrigger(IsBladeSkillAnimation);
        StartCoroutine(BaseInfo.playerInfo.m_Player.SetEffect(0, 1.5f));
    }

    public override void DevilSlashSkill(BaseState state)
    {
        Debug.Log("Devil Slash Skill On");
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 0.5f);
        BaseInfo.playerInfo.m_Anim.SetTrigger(IsBladeSkillAnimation);
        GameObject go = ResourcesManager.Instance.Instantiate("Player/Effect/Skill/DevilSlash", BaseInfo.playerInfo.transform);
        StartCoroutine(BaseInfo.playerInfo.m_Player.DesSkill(go, 4));
    }

    public override void LightRefereeSkill(BaseState state)
    {
        Debug.Log("Light Referee Skill On");
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 0.5f);
        BaseInfo.playerInfo.m_Anim.SetTrigger(IsUltiSkillAnimation);
        GameObject go = ResourcesManager.Instance.Instantiate("Player/Effect/Skill/LightReferee" , BaseInfo.playerInfo.transform);
        StartCoroutine(SkillSound(15 ,"Arrow Skill", 0.1f));
        StartCoroutine(BaseInfo.playerInfo.m_Player.DesSkill(go, 10));
    }

    public override void AngelSkill(BaseState state)
    {
        Debug.Log("Angel Skill On");
        SoundManager.Instance.Play("Effect/Angel Skill");
        BaseInfo.playerInfo.m_Anim.SetFloat(AttackSpeedAnimation, 2f);
        BaseInfo.playerInfo.m_Anim.SetTrigger(IsBuffSkillAnimation);
        StartCoroutine(BaseInfo.playerInfo.m_Player.SetEffect(1, 5));
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

    private IEnumerator SkillSound(int count, string path, float retime)
    {
        for (int i = 0; i < count; i++)
        {
            SoundManager.Instance.Play($"Effect/{path}");
            yield return new WaitForSeconds(retime);
        }
    }
}
