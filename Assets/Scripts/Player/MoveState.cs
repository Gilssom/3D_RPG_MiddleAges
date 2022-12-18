using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public MoveState(Player playerCtrl) : base(playerCtrl)
        {

        }

        public override void OnEnterState()
        {

        }

        public override void OnUpdateState()
        {
            float curMoveSpeed = m_PlayerController.playerInfo.MoveSpeed;

            PlayerInfo.Instance.m_Anim.SetFloat("Ymove", m_PlayerController.VAxis * (m_PlayerController.LSDown ? 2 : 1));
            PlayerInfo.Instance.m_Anim.SetFloat("Xmove", m_PlayerController.HAxis * (m_PlayerController.LSDown ? 2 : 1));
            PlayerInfo.Instance.m_Anim.SetFloat("MoveSpeed", curMoveSpeed);

            m_PlayerController.LookAt(m_PlayerController.m_LookForward);
            m_PlayerController.transform.position += m_PlayerController.m_CulatedDirection * Time.deltaTime * (m_PlayerController.LSDown ? curMoveSpeed * 2 : curMoveSpeed);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {

        }
    }
}

