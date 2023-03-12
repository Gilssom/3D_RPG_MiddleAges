using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Condition/IsQuestComplete", fileName = "IsQuestComlete")]
public class isQuestComplete : Condition
{
    [SerializeField]
    private Quest m_Target;

    public override bool IsPass(Quest quest)
        => QuestSystem.Instance.ContainsInCompleteQuests(m_Target);
}
