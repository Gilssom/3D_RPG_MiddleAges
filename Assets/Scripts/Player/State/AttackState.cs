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
            ULTIMATE,
            Blade,
            Slash,
            Referee,
            Angel,
        }

        public static AttackName m_AttackName;

        public static bool isAttack = false;

        public AttackState(Player playerCtrl) : base(playerCtrl) { }

        public override void OnEnterState()
        {
            switch (m_AttackName)
            {
                case AttackName.ATTACK:
                    isAttack = true;
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.Attack(this);
                    break;
                case AttackName.CHARGE:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.ChargingAttack(this);
                    break;
                case AttackName.KICK:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.KickAttack(this);
                    break;
                case AttackName.ULTIMATE:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.UltimateSkill(this);
                    break;
                case AttackName.Blade:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.BladeSkill(this);
                    break;
                case AttackName.Slash:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.DevilSlashSkill(this);
                    break;
                case AttackName.Referee:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.LightRefereeSkill(this);
                    break;
                case AttackName.Angel:
                    m_PlayerController.playerInfo.m_WeaponManager.m_Weapon?.AngelSkill(this);
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