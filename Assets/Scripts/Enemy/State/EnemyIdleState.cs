using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class EnemyIdleState : BaseState
    {
        public EnemyIdleState(Enemy enemyCtrl) : base(enemyCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log($"{m_EnemyController.enemyInfo.Name}의 현재 상태는 Idle !");
        }

        public override void OnUpdateState()
        {
            m_EnemyController.PlayerCheck();
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            Debug.Log("Idle Exit State");
        }
    }
}
