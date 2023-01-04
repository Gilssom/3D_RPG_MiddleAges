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
            Debug.Log("Idle Enter State");
        }

        public override void OnUpdateState()
        {
            Debug.Log($"{base.m_EnemyController.enemyInfo.Name}의 현재 상태는 Idle !");
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
