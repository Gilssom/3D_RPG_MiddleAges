using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CharacterController
{
    public class StateMachine
    {
        public BaseState CurState { get; private set; } // 현재 상태
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurState = GetState(stateName);
        }

        public void AddState(StateName stateName, BaseState state) // 상태 등록
        {
            if(!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        public BaseState GetState(StateName stateName) // 상태 꺼내오기
        {
            if (states.TryGetValue(stateName, out BaseState state))
                return state;

            return null;
        }

        public void DeleteState(StateName removeStateName) // 상태 삭제
        {
            if(states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName) // 상태 전환
        {
            CurState?.OnExitState(); // 현재 상태를 종료함

            if(states.TryGetValue(nextStateName, out BaseState newState)) // 상태 전환
            {
                CurState = newState;
            }

            CurState?.OnEnterState(); // 다음 상태 진입 실행
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
