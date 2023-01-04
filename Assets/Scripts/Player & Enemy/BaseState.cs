using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public enum StateName
    {
        MOVE = 100,
        DASH,
        ATTACK,
        IDLE,
        DIE
    }

    public abstract class BaseState
    {
        protected Player m_PlayerController { get; private set; }
        protected Enemy m_EnemyController { get; private set; }

        public BaseState(Player playerCtrl)
        {
            this.m_PlayerController = playerCtrl;
        }

        public BaseState(Enemy enemyCtrl)
        {
            this.m_EnemyController = enemyCtrl;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}

