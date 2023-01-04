using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class EnemyAttackState : BaseState
    {
        //public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
        public static bool isAttack = false;

        public EnemyAttackState(Enemy enemyCtrl) : base(enemyCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log("Attack Enter State");
            isAttack = true;
            m_EnemyController.enemyInfo.m_Anim.SetBool("isAttack", true);
            m_EnemyController.StartCoroutine(m_EnemyController.AttackCoroutine());
        }

        public override void OnUpdateState()
        {
            Debug.Log($"{base.m_EnemyController.enemyInfo.Name}의 현재 상태는 Attack !");
            m_EnemyController.LookAt();
            m_EnemyController.MoveStop(true);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            Debug.Log("Attack Exit State");
            m_EnemyController.enemyInfo.m_Anim.SetBool("isAttack", false);
        }
    }
}
