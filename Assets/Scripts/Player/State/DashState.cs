using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DashState : BaseState
    {
        public int m_CurDashCount { get; set; } = 0;
        public bool m_CanAddInputBuffer { get; set; } // Buffer Input 이 가능한지
        public bool m_CanDashAttack { get; set; }
        public static bool m_IsDash { get; set; }
        public int Hash_DashTrigger { get; private set; }
        public int Hash_IsDashBool { get; private set; }
        public int Hash_DashPlaySpeedFloat { get; private set; }
        public Queue<Vector3> m_InputDirBuffer { get; private set; }

        public const float DEFAULT_ANIMATION_SPEED = 2f;
        public readonly float m_DashPower;
        public readonly float m_DashTetanyTime;
        public readonly float m_DashCoolTime;

        public DashState(Player playerCtrl, float dashPower, float dashTetanyTime, float dashCoolTime) : base(playerCtrl)
        {
            m_InputDirBuffer = new Queue<Vector3>();
            this.m_DashPower = dashPower;
            this.m_DashTetanyTime = dashTetanyTime;
            this.m_DashCoolTime = dashCoolTime;
            Hash_DashTrigger = Animator.StringToHash("Dash");
            Hash_IsDashBool = Animator.StringToHash("isDashing");
            Hash_DashPlaySpeedFloat = Animator.StringToHash("DashPlaySpeed");
        }

        public override void OnEnterState()
        {
            m_IsDash = true;
            m_CanAddInputBuffer = false;
            m_CanDashAttack = false;
            Dash();
        }

        private void Dash()
        {
            Vector3 DashDir = (m_PlayerController.m_CulatedDirection == Vector3.zero) ? m_PlayerController.playerInfo.transform.forward : m_PlayerController.m_CulatedDirection;

            m_PlayerController.playerInfo.m_Anim.SetBool(Hash_IsDashBool, true);
            m_PlayerController.playerInfo.m_Anim.SetTrigger(Hash_DashTrigger);

            m_PlayerController.playerInfo.m_Rigid.velocity = DashDir * m_DashPower;

            float dashAnimPlaySpeed = DEFAULT_ANIMATION_SPEED + (m_PlayerController.playerInfo.MoveSpeed);
            m_PlayerController.playerInfo.m_Anim.SetFloat(Hash_DashPlaySpeedFloat, dashAnimPlaySpeed);
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            m_IsDash = false;
            m_PlayerController.playerInfo.m_Rigid.velocity = Vector3.zero;
            m_PlayerController.playerInfo.m_Anim.SetBool(Hash_IsDashBool, false);
        }
    }
}
