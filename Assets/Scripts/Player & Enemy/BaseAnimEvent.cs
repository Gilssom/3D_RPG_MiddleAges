using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public abstract class BaseAnimEvent : MonoBehaviour
    {
        [Header("공격 히트 박스")]
        [SerializeField, Tooltip("히트 박스 콜라이더")]
        protected Collider m_AttackArea;
        [SerializeField, Tooltip("히트 박스를 다시 끄는 시간")]
        protected float m_HitCheckTime;
        [SerializeField, Tooltip("연속 콤보 히트 박스 콜라이더")]
        protected Collider m_BladeHitArea;
        [SerializeField, Tooltip("연속 콤보 히트 박스를 다시 끄는 시간")]
        protected float m_BladeHitCheckTime;

        protected WaitForSeconds m_CheckTime;
        protected WaitForSeconds m_BladeCheckTime;

        private void Start()
        {
            Init();
        }

        protected abstract void Init();
        protected abstract void OnFinishedAttack();
        protected abstract void OnForwardAttack(float Power);
        protected abstract void OffForwardAttack();
        protected abstract IEnumerator AttackArea(int attackNumber);
        protected abstract void TestAttackEffect(int AttackNumber);
    }
}
