using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillClasses : MonoBehaviour
{
    
}

public interface Skill
{

}

[System.Serializable]
public abstract class SkillBase : Skill
{
    public string m_Name;
    public Sprite m_SkillIcon;
    public Characters m_WhoPlayer;
    public string m_Description;

    public virtual void SkillExecute() { }
}

public enum Characters
{
    NightMare, Demonic,
}

[System.Serializable]
public abstract class ActiveSkill : SkillBase
{
    public int m_UseCost;
    public float m_CoolTime;
}

[System.Serializable]
public abstract class PassiveSkill : SkillBase
{
    public int m_UsedLevel;
    public string m_FlavorText;
}

[System.Serializable]
public abstract class NMActiveSkill : ActiveSkill
{
    public NMSkillCategory m_Category;
    public NMSkillResources m_RequireResource;
}

// 해당 스킬 소모 자원 : 체력, 각성의 돌
public enum NMSkillResources
{
    Health, SkillStone
}

// 스킬 카테고리 : 공격형, 방어형, 유틸형
public enum NMSkillCategory
{
    Attacker, Defenser, Utilliter
}

[System.Serializable]
public abstract class NMPassiveSkill : PassiveSkill
{
    public NMSkillCategory m_Category;
}

// 고대의 창 : 공격형
[System.Serializable]
public class AncientSpear : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        BaseInfo.playerInfo.m_Player.UltimateSkill();
        InventoryManager.Instance.m_SkillQuickSlot.ResetCoolTime(m_CoolTime);
    }
}

// 임페일 블레이드 : 공격형
[System.Serializable]
public class ImpaleBlade : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        BaseInfo.playerInfo.m_Player.BladeSkill();
        InventoryManager.Instance.m_SkillQuickSlot.ResetCoolTime(m_CoolTime);
    }
}

// 빛의 심판 : 공격형
[System.Serializable]
public class LightReferee : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        BaseInfo.playerInfo.m_Player.LigthRefereeSkill();
        InventoryManager.Instance.m_SkillQuickSlot.ResetCoolTime(m_CoolTime);
    }
}

// 데빌 슬래쉬 : 공격형
[System.Serializable]
public class DevilSlash : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        BaseInfo.playerInfo.m_Player.DevilSlashSkill();
        InventoryManager.Instance.m_SkillQuickSlot.ResetCoolTime(m_CoolTime);
    }
}

// 천사의 포옹 : 방어형
[System.Serializable]
public class AngelEmbrace : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        BaseInfo.playerInfo.m_Player.AngelSkill();
        InventoryManager.Instance.m_SkillQuickSlot.ResetCoolTime(m_CoolTime);
    }
}

// 바람의 속삭임 : 유틸형
[System.Serializable]
public class WhispersWind : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        BaseInfo.playerInfo.m_Player.WhispersSkill();
        InventoryManager.Instance.m_SkillQuickSlot.ResetCoolTime(m_CoolTime);
    }
}

// 속전속결 : 공격형
[System.Serializable]
public class QuickDecision: NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("레벨 부족 - 속전속결 사용 불가");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.MoveSpeed += 0.2f;
        Debug.Log("속전속결");
    }
}

// 기습공격 : 공격형
[System.Serializable]
public class SurpriseAttack : NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("레벨 부족 - 기습공격 사용 불가");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.CriticalChance += 5;
        BaseInfo.playerInfo.m_Plus_CriChance += 5;
        Debug.Log("기습공격");
    }
}

// 중갑착용 : 방어형
[System.Serializable]
public class WearingHeavyArmor : NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("레벨 부족 - 중갑착용 사용 불가");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.Defense += 3;
        BaseInfo.playerInfo.m_Plus_Defense += 3;
        Debug.Log("중갑착용");
    }
}

// 생명의 축복 : 유틸형
[System.Serializable]
public class BlessingLife : NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("레벨 부족 - 생명의 축복 사용 불가");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.m_Plus_IncreaseHp += (int)(BaseInfo.playerInfo.MaxHp * 0.03f);
        Debug.Log("생명의 축복");
    }
}