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

// �ش� ��ų �Ҹ� �ڿ� : ü��, ������ ��
public enum NMSkillResources
{
    Health, SkillStone
}

// ��ų ī�װ� : ������, �����, ��ƿ��
public enum NMSkillCategory
{
    Attacker, Defenser, Utilliter
}

[System.Serializable]
public abstract class NMPassiveSkill : PassiveSkill
{
    public NMSkillCategory m_Category;
}

// ����� â : ������
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

// ������ ���̵� : ������
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

// ���� ���� : ������
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

// ���� ������ : ������
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

// õ���� ���� : �����
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

// �ٶ��� �ӻ��� : ��ƿ��
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

// �����Ӱ� : ������
[System.Serializable]
public class QuickDecision: NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("���� ���� - �����Ӱ� ��� �Ұ�");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.MoveSpeed += 0.2f;
        Debug.Log("�����Ӱ�");
    }
}

// ������� : ������
[System.Serializable]
public class SurpriseAttack : NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("���� ���� - ������� ��� �Ұ�");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.CriticalChance += 5;
        BaseInfo.playerInfo.m_Plus_CriChance += 5;
        Debug.Log("�������");
    }
}

// �߰����� : �����
[System.Serializable]
public class WearingHeavyArmor : NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("���� ���� - �߰����� ��� �Ұ�");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.Defense += 3;
        BaseInfo.playerInfo.m_Plus_Defense += 3;
        Debug.Log("�߰�����");
    }
}

// ������ �ູ : ��ƿ��
[System.Serializable]
public class BlessingLife : NMPassiveSkill
{
    public override void SkillExecute()
    {
        if (m_UsedLevel > BaseInfo.playerInfo.Level)
        {
            Debug.Log("���� ���� - ������ �ູ ��� �Ұ�");
            return;
        }

        base.SkillExecute();
        BaseInfo.playerInfo.m_Plus_IncreaseHp += (int)(BaseInfo.playerInfo.MaxHp * 0.03f);
        Debug.Log("������ �ູ");
    }
}