using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class BossAnimationEvent : BaseAnimEvent
    {
        public BossInfo bossInfo { get; private set; }

        [Header("보스 전용 히트 박스")]
        [SerializeField, Tooltip("히트 박스 콜라이더")]
        private Collider[] m_BossAttackArea;

        protected override void Init()
        {
            m_CheckTime = new WaitForSeconds(m_HitCheckTime);
            bossInfo = GetComponent<BossInfo>();
        }

        protected override void SoundEffect(string soundName)
        {
            SoundManager.Instance.Play($"Effect/{soundName}");
        }

        protected override void OnFinishedAttack()
        {
            bossInfo.stateMachine.ChangeState(StateName.IDLE);
        }

        protected override void OnForwardAttack(float Power)
        {
            bossInfo.m_Rigid.velocity = bossInfo.transform.forward * Power;
        }

        protected override void OffForwardAttack()
        {
            bossInfo.m_Rigid.velocity = Vector3.zero;
        }

        protected override IEnumerator AttackArea(int attackNumber)
        {
            m_BossAttackArea[attackNumber].enabled = true;
            yield return m_CheckTime;
            m_BossAttackArea[attackNumber].enabled = false;
        }

        protected override void TestAttackEffect(int AttackNumber)
        {
            bossInfo.m_EffectList[AttackNumber].Play();
        }

        public void EnemyReadyAttackFalse()
        {
            bossInfo.ReadyAttack = false;
        }

        public void OnOffCoverEffect(int ObjectNumber)
        {
            bossInfo.m_ObjectList[ObjectNumber].SetActive(bossInfo.Cover);
        }
    }
}
