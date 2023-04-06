using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CharacterController
{
    public class StateMachine
    {
        public BaseState CurState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurState = GetState(stateName);
        }

        public void AddState(StateName stateName, BaseState state)
        {
            if(!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        public BaseState GetState(StateName stateName)
        {
            if (states.TryGetValue(stateName, out BaseState state))
                return state;

            return null;
        }

        public void DeleteState(StateName removeStateName)
        {
            if(states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)
        {
            CurState?.OnExitState();

            if(states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurState = newState;
            }

            CurState?.OnEnterState();
        }

        public void UpdateState()
        {
            CurState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            CurState?.OnFixedUpdateState();
        }
    }
}
