using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class BossAttackState : BaseState
    {
        public readonly int IsAttackAnimation = Animator.StringToHash("isAttack");
        public readonly int IsSpecialAttackAnimation = Animator.StringToHash("isSpecialAttack");
        public readonly int IsEpicAttackAnimation = Animator.StringToHash("isEpicAttack");
        public readonly int IsCoverAnimation = Animator.StringToHash("isCover");
        public static bool isAttack = false;

        public BossAttackState(Boss bossCtrl) : base(bossCtrl)
        {

        }

        public override void OnEnterState()
        {
            Debug.Log($"{m_BossController.bossInfo.Name}의 현재 상태는 Attack !");
            int RandomAttack = Random.Range(0, 11);

            if (m_BossController.bossInfo.ReadyAttack == true)
            {
                if(RandomAttack < 2)
                {
                    m_BossController.bossInfo.m_Anim.SetBool(IsCoverAnimation, true);
                }
                else if(RandomAttack < 5)
                {
                    m_BossController.bossInfo.m_Anim.SetBool(IsAttackAnimation, true);
                }
                else if(RandomAttack < 8)
                {
                    m_BossController.bossInfo.m_Anim.SetBool(IsEpicAttackAnimation, true);
                }
                else
                {
                    m_BossController.bossInfo.m_Anim.SetBool(IsSpecialAttackAnimation, true);
                }

                m_BossController.StartCoroutine(m_BossController.AttackCoroutine());
            }
        }

        public override void OnUpdateState()
        {
            m_BossController.LookAt();
            m_BossController.MoveStop(true);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            m_BossController.bossInfo.m_Anim.SetBool(IsAttackAnimation, false);
            m_BossController.bossInfo.m_Anim.SetBool(IsEpicAttackAnimation, false);
            m_BossController.bossInfo.m_Anim.SetBool(IsSpecialAttackAnimation, false);
            m_BossController.bossInfo.m_Anim.SetBool(IsCoverAnimation, false);
        }
    }
}
