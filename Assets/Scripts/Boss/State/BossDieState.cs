using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class BossDieState : BaseState
    {
        public readonly int IsDieAnimation = Animator.StringToHash("Die"); 

        public BossDieState(Boss bossCtrl) : base(bossCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log($"{m_BossController.bossInfo.Name}의 현재 상태는 Die !");
            m_BossController.bossInfo.m_Anim.Play(IsDieAnimation);
            m_BossController.bossInfo.m_CapsuleCollider.enabled = false;
        }

        public override void OnUpdateState()
        {
            float Speed = m_BossController.bossInfo.m_DissolveSpeed;

            m_BossController.bossInfo.m_DissolveCutoff += Speed;

            if (m_BossController.bossInfo.m_DissolveCutoff >= 1)
            {
                OnExitState();
                return;
            }
            else if (m_BossController.bossInfo.m_DissolveCutoff != 1)
            {
                m_BossController.bossInfo.m_Material.SetFloat("_Dissolve", m_BossController.bossInfo.m_DissolveCutoff);
            }
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            // 아이템 드랍
            m_BossController.bossInfo.m_LootDrop.SpawnDrop(m_BossController.transform, m_BossController.bossInfo.RandomDropCount, m_BossController.bossInfo.ItemDropRange);

            GameManager.Instance.Despawn(m_BossController.gameObject);
        }
    }
}
