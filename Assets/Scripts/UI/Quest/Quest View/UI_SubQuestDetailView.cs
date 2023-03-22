using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SubQuestDetailView : UI_QuestDetailView
{
    [SerializeField]
    private Button m_AcceptButton;

    private Quest m_CurQuest;

    private void Start()
    {
        m_AcceptButton.onClick.AddListener(AcceptQuest);
        m_CancelButton.onClick.AddListener(RefuseQuest);
    }

    public override void Show(Quest quest)
    {
        base.Show(quest);
        m_CurQuest = quest;
    }

    private void AcceptQuest()
    {
        QuestSystem.Instance.Register(m_CurQuest);
        InventoryManager.Instance.TryOpenSubQuestSystem();
        BaseInfo.playerInfo.m_Player.m_NearNpc.isSubQuest = false;

        foreach (var quest in QuestSystem.Instance.p_ActiveQuests)
        {
            if (quest.p_DisPlayName == m_CurQuest.p_DisPlayName)
            {
                Debug.Log($"{quest.p_DisPlayName} {quest.m_State}");
                BaseInfo.playerInfo.m_Player.m_NearNpc.SubQuestMarker(quest);
                break;
            }
        }
    }

    private void RefuseQuest()
    {
        InventoryManager.Instance.TryOpenSubQuestSystem();
    }
}
