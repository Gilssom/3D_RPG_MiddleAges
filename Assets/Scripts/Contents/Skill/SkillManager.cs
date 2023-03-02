using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletomManager<SkillManager>
{
    public AncientSpear AncientSpear;
    public ImpaleBlade ImpaleBlade;
    public AngelEmbrace AngelEmbrace;
    public WhispersWind WhispersWind;
    public LightReferee LightReferee;
    public DevilSlash DevilSlash;
    public QuickDecision QuickDecision;
    public SurpriseAttack SurpriseAttack;
    public WearingHeavyArmor WearingHeavyArmor;
    public BlessingLife BlessingLife;

    public List<Skill> AllSkills = new List<Skill>();
    public List<NMActiveSkill> NMActiveSkills = new List<NMActiveSkill>();
    public List<NMPassiveSkill> NMPassiveSkills = new List<NMPassiveSkill>();

    void Awake()
    {
        Setting();
    }

    public void Setting()
    {
        AllSkills.Add(AncientSpear);
        AllSkills.Add(ImpaleBlade);
        AllSkills.Add(AngelEmbrace);
        AllSkills.Add(WhispersWind);
        AllSkills.Add(LightReferee);
        AllSkills.Add(DevilSlash);
        AllSkills.Add(QuickDecision);
        AllSkills.Add(SurpriseAttack);
        AllSkills.Add(WearingHeavyArmor);
        AllSkills.Add(BlessingLife);

        foreach (Skill sk in AllSkills)
        {
            // SkillClasses ���� ������ ��ų�� ���� Ư���� �°�
            // �ν�����â�� ���� ������ �ڵ������� �������ش�.
            if (sk is NMActiveSkill)
            {
                NMActiveSkill thisSkill = sk as NMActiveSkill;
                thisSkill.m_WhoPlayer = Characters.NightMare;
                NMActiveSkills.Add(thisSkill);
            }

            if (sk is NMPassiveSkill)
            {
                NMPassiveSkill thisSkill = sk as NMPassiveSkill;
                thisSkill.m_WhoPlayer = Characters.NightMare;
                NMPassiveSkills.Add(thisSkill);
            }
        }
    }
}
