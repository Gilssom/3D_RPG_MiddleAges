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
            Debug.Log($"{m_BossController.bossInfo.Name}의 현재 상태는 Move !");

            m_BossController.bossInfo.m_Anim.SetBool(IsWalkAnimation, true);
        }

        public override void OnUpdateState()
        {
            float curMoveSpeed = m_BossController.bossInfo.MoveSpeed;

            m_BossController.bossInfo.m_Anim.SetFloat(IsMoveSpeed, curMoveSpeed);
            m_BossController.Move();
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