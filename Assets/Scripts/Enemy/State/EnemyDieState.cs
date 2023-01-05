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
            Debug.Log($"{m_EnemyController.enemyInfo.Name}의 현재 상태는 Die !");
            m_EnemyController.enemyInfo.m_Anim.Play(IsDieAnimation);
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {

        }
    }
}
