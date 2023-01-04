using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class EnemyDieState : BaseState
    {
        public readonly int IsDieAnimation = Animator.StringToHash("Mutant_Dying");

        public EnemyDieState(Enemy enemyCtrl) : base(enemyCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log("Die Enter State");
            m_EnemyController.enemyInfo.m_Anim.Play(IsDieAnimation);
        }

        public override void OnUpdateState()
        {
            Debug.Log($"{base.m_EnemyController.enemyInfo.Name}의 현재 상태는 Die !");
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            Debug.Log("Attack Die State");
        }
    }
}
