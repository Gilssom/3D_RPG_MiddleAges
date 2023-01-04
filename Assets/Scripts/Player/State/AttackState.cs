using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public enum AttackName
        {
            ATTACK = 200,
            CHARGE,
            KICK,
        }

        public static AttackName m_AttackName;

        public static bool isAttack = false;
        public const float m_CanReInputTime = 1f;

        public AttackState(Player playerCtrl) : base(playerCtrl) { }

        public override void OnEnterState()
        {
            switch (m_AttackName)
            {
                case AttackName.ATTACK:
                    isAttack = true;
                    PlayerInfo.Instance.m_WeaponManager.m_Weapon?.Attack(this);
                    break;
                case AttackName.CHARGE:
                    PlayerInfo.Instance.m_WeaponManager.m_Weapon?.ChargingAttack(this);
                    break;
                case AttackName.KICK:
                    PlayerInfo.Instance.m_WeaponManager.m_Weapon?.KickAttack(this);
                    break;
            }
        }

        public override void OnExitState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {

        }
    }
}