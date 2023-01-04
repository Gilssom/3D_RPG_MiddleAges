using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class EnemyMoveState : BaseState
    {
        public readonly int IsWalkAnimation = Animator.StringToHash("isWalk");
        public readonly int IsMoveSpeed = Animator.StringToHash("MoveSpeed");

        public EnemyMoveState(Enemy enemyCtrl) : base(enemyCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log("Move Enter State");

            m_EnemyController.enemyInfo.m_Anim.SetBool(IsWalkAnimation, true);
        }

        public override void OnUpdateState()
        {
            Debug.Log($"{m_EnemyController.enemyInfo.Name}의 현재 상태는 Move !");

            float curMoveSpeed = m_EnemyController.enemyInfo.MoveSpeed;

            m_EnemyController.enemyInfo.m_Anim.SetFloat(IsMoveSpeed, curMoveSpeed);
            m_EnemyController.Move();
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            Debug.Log("Move Exit State");
            m_EnemyController.enemyInfo.m_Anim.SetBool(IsWalkAnimation, false);
        }
    }
}
