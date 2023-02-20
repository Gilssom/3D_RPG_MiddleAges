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
            Debug.Log($"{m_EnemyController.enemyInfo.Name}�� ���� ���´� Die !");
            m_EnemyController.enemyInfo.m_Anim.Play(IsDieAnimation);
            m_EnemyController.enemyInfo.m_CapsuleCollider.enabled = false;
        }

        public override void OnUpdateState()
        {
            float Speed = m_EnemyController.enemyInfo.m_DissolveSpeed;

            m_EnemyController.enemyInfo.m_DissolveCutoff += Speed;

            if (m_EnemyController.enemyInfo.m_DissolveCutoff >= 1)
            {
                OnExitState();
                return;
            }
            else if (m_EnemyController.enemyInfo.m_DissolveCutoff != 1)
            {
                m_EnemyController.enemyInfo.m_Material.SetFloat("_Dissolve", m_EnemyController.enemyInfo.m_DissolveCutoff);
            }
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            // ������ ���
            m_EnemyController.enemyInfo.m_LootDrop.SpawnDrop(m_EnemyController.transform, m_EnemyController.enemyInfo.RandomDropCount, m_EnemyController.enemyInfo.ItemDropRange);

            GameManager.Instance.Despawn(m_EnemyController.gameObject);
        }
    }
}
