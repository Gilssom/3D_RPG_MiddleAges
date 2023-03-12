using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Quest[] m_Quest;

    private void Start()
    {
        foreach (var quest in m_Quest)
        {
            if (quest.p_IsAcceptable && !QuestSystem.Instance.ContainsInCompleteQuests(quest))
                QuestSystem.Instance.Register(quest);
        }
    }
}
