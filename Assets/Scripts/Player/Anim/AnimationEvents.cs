using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AnimationEvents : BaseAnimEvent
    {
        public PlayerInfo playerInfo { get; private set; }

        public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
        public readonly int IsChargeAnimation = Animator.StringToHash("isCharge");
        public readonly int IsKickAnimation = Animator.StringToHash("isKick");

        protected override void Init()
        {
            m_CheckTime = new WaitForSeconds(m_HitCheckTime);
            playerInfo = GetComponent<PlayerInfo>();
        }

        protected override void OnFinishedAttack()
        {
            AttackState.isAttack = false;
            playerInfo.m_Anim.SetBool(IsAttackAnimation, false);
            playerInfo.m_Anim.SetBool(IsKickAnimation, false);
            playerInfo.m_Anim.SetBool(IsChargeAnimation, false);
            playerInfo.stateMachine.ChangeState(StateName.MOVE);
        }

        protected override void OnForwardAttack(float Power)
        {
            playerInfo.m_Rigid.velocity = playerInfo.transform.forward * Power;
        }

        protected override void OffForwardAttack()
        {
            playerInfo.m_Rigid.velocity = Vector3.zero;
        }

        protected override IEnumerator AttackArea()
        {
            m_AttackArea.enabled = true;
            yield return m_CheckTime;
            m_AttackArea.enabled = false;
        }

        protected override void TestAttackEffect(int AttackNumber)
        {
            playerInfo.m_EffectList[AttackNumber].Play();
        }
    }
}
