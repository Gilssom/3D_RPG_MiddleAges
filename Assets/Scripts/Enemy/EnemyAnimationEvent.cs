using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class EnemyAnimationEvent : BaseAnimEvent
{
    //public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");

    protected override void Init()
    {
        m_CheckTime = new WaitForSeconds(m_HitCheckTime);
    }

    protected override void OnFinishedAttack()
    {
        Debug.LogWarning("OnFinished");
        EnemyAttackState.isAttack = false;
        EnemyInfo.Instance.m_Anim.SetBool("isAttack", false);
        EnemyInfo.Instance.stateMachine.ChangeState(StateName.IDLE);
    }

    protected override void OnForwardAttack(float Power)
    {
        EnemyInfo.Instance.m_Rigid.velocity = EnemyInfo.Instance.transform.forward * Power;
    }

    protected override void OffForwardAttack()
    {
        EnemyInfo.Instance.m_Rigid.velocity = Vector3.zero;
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
