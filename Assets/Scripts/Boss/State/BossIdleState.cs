using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class BossIdleState : BaseState
    {
        public readonly int IsIdle_2Animation = Animator.StringToHash("isNoTarget");

        public BossIdleState(Boss bossCtrl) : base(bossCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log($"{m_BossController.bossInfo.Name}의 현재 상태는 Idle !");
        }

        public override void OnUpdateState()
        {
            m_BossController.PlayerCheck();

            if(m_BossController.m_Target)
                m_BossController.bossInfo.m_Anim.SetBool(IsIdle_2Animation, true);
            else
                m_BossController.bossInfo.m_Anim.SetBool(IsIdle_2Animation, false);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {

        }
    }
}
