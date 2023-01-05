using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class EnemyAnimationEvent : BaseAnimEvent
    {
        public EnemyInfo enemyInfo { get; private set; }

        protected override void Init()
        {
            m_CheckTime = new WaitForSeconds(m_HitCheckTime);
            enemyInfo = GetComponent<EnemyInfo>();
        }

        protected override void OnFinishedAttack()
        {
            enemyInfo.stateMachine.ChangeState(StateName.IDLE);
        }

        protected override void OnForwardAttack(float Power)
        {
            enemyInfo.m_Rigid.velocity = enemyInfo.transform.forward * Power;
        }

        protected override void OffForwardAttack()
        {
            enemyInfo.m_Rigid.velocity = Vector3.zero;
        }

        protected override IEnumerator AttackArea()
        {
            m_AttackArea.enabled = true;
            yield return m_CheckTime;
            m_AttackArea.enabled = false;
        }

        protected override void TestAttackEffect(int AttackNumber)
        {
            GameObject Effect = ObjectPoolManager.Instance.m_ObjectPoolList[AttackNumber].Dequeue();
            Effect.SetActive(true);

            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(1.5f, AttackNumber, Effect));
        }

        public void EnemyReadyAttackFalse()
        {
            enemyInfo.ReadyAttack = false;
        }
    }
}
