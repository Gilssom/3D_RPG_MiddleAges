using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public abstract class BaseAnimEvent : MonoBehaviour
    {
        [Header("���� ��Ʈ �ڽ�")]
        [SerializeField, Tooltip("��Ʈ �ڽ� �ݶ��̴�")]
        protected Collider m_AttackArea;
        [SerializeField, Tooltip("��Ʈ �ڽ��� �ٽ� ���� �ð�")]
        protected float m_HitCheckTime;
        [SerializeField, Tooltip("���� �޺� ��Ʈ �ڽ� �ݶ��̴�")]
        protected Collider m_BladeHitArea;
        [SerializeField, Tooltip("���� �޺� ��Ʈ �ڽ��� �ٽ� ���� �ð�")]
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
