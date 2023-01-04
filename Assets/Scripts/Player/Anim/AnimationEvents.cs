using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class AnimationEvents : BaseAnimEvent
{
    public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
    public readonly int IsChargeAnimation = Animator.StringToHash("isCharge");
    public readonly int IsKickAnimation = Animator.StringToHash("isKick");

    protected override void Init()
    {
        m_CheckTime = new WaitForSeconds(m_HitCheckTime);
    }

    protected override void OnFinishedAttack()
    {
        AttackState.isAttack = false;
        PlayerInfo.Instance.m_Anim.SetBool(IsAttackAnimation, false);
        PlayerInfo.Instance.m_Anim.SetBool(IsKickAnimation, false);
        PlayerInfo.Instance.m_Anim.SetBool(IsChargeAnimation, false);
        PlayerInfo.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    protected override void OnForwardAttack(float Power)
    {
        PlayerInfo.Instance.m_Rigid.velocity = PlayerInfo.Instance.transform.forward * Power;
    }

    protected override void OffForwardAttack()
    {
        PlayerInfo.Instance.m_Rigid.velocity = Vector3.zero;
    }

    protected override IEnumerator AttackArea()
    {
        m_AttackArea.enabled = true;
        Debug.Log(m_AttackArea.enabled);
        yield return m_CheckTime;
        m_AttackArea.enabled = false;
        Debug.Log(m_AttackArea.enabled);
    }

    protected override void TestAttackEffect(int AttackNumber)
    {
        GameObject Effect = ObjectPoolManager.Instance.m_ObjectPoolList[AttackNumber].Dequeue();
        Effect.SetActive(true);

        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(1.5f, AttackNumber, Effect));
    }
}
