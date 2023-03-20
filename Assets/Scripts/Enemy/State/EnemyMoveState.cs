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
            Debug.Log($"{m_EnemyController.enemyInfo.Name}의 현재 상태는 Move !");

            m_EnemyController.enemyInfo.m_Anim.SetBool(IsWalkAnimation, true);
        }

        public override void OnUpdateState()
        {
            float curMoveSpeed = m_EnemyController.enemyInfo.MoveSpeed;

            m_EnemyController.enemyInfo.m_Anim.SetFloat(IsMoveSpeed, curMoveSpeed);
            m_EnemyController.PlayerCheck();

            if (m_EnemyController.m_Target)
                m_EnemyController.Move(m_EnemyController.m_Target.transform.position);
            else
                m_EnemyController.Move(m_EnemyController.m_SpawnTransform);

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            m_EnemyController.enemyInfo.m_Anim.SetBool(IsWalkAnimation, false);
        }
    }
}
