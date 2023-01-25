using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class BossRunState : BaseState
    {
        public readonly int IsRunAnimation = Animator.StringToHash("isRun");
        public readonly int IsMoveSpeed = Animator.StringToHash("MoveSpeed");

        public BossRunState(Boss bossCtrl) : base(bossCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log($"{m_BossController.bossInfo.Name}의 현재 상태는 Run !");
            m_BossController.bossInfo.MoveSpeed = 1.5f;
            m_BossController.bossInfo.m_Anim.SetBool(IsRunAnimation, true);
        }

        public override void OnUpdateState()
        {
            float curMoveSpeed = m_BossController.bossInfo.MoveSpeed;

            m_BossController.bossInfo.m_Anim.SetFloat(IsMoveSpeed, curMoveSpeed);

            if (!m_BossController.bossInfo.RunAway)
                m_BossController.Move(m_BossController.m_Target.transform.position);
            else
                m_BossController.CheckRanTargetEnd();

            m_BossController.PlayerCheck();
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            m_BossController.bossInfo.m_Anim.SetBool(IsRunAnimation, false);
        }
    }
}
