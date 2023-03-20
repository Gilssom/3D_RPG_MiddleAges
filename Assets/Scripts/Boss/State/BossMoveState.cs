using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class BossMoveState : BaseState
    {
        public readonly int IsWalkAnimation = Animator.StringToHash("isWalk");
        public readonly int IsMoveSpeed = Animator.StringToHash("MoveSpeed");

        public BossMoveState(Boss bossCtrl) : base(bossCtrl)
        {

        }

        public override void OnEnterState()
        {
            m_BossController.bossInfo.MoveSpeed = 1;
            m_BossController.bossInfo.m_Anim.SetBool(IsWalkAnimation, true);
        }

        public override void OnUpdateState()
        {
            float curMoveSpeed = m_BossController.bossInfo.MoveSpeed;

            m_BossController.bossInfo.m_Anim.SetFloat(IsMoveSpeed, curMoveSpeed);

            if (!m_BossController.bossInfo.RunAway)
            {
                if (m_BossController.m_Target)
                    m_BossController.Move(m_BossController.m_Target.transform.position);
                else
                    m_BossController.Move(m_BossController.m_SpawnTransform);
            }

            m_BossController.PlayerCheck();
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            m_BossController.bossInfo.m_Anim.SetBool(IsWalkAnimation, false);
        }
    }
}