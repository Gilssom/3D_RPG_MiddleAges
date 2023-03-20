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
            m_EnemyController.MoveStop(true);
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

        }
    }
}
