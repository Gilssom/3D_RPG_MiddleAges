using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_QuestListViewCtrl : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup m_TabGroup;
    [SerializeField]
    private UI_QuestListView m_ActiveQuestListView;
    [SerializeField]
    private UI_QuestListView m_CompletedQuestListView;

    public IEnumerable<Toggle> m_Tabs => m_TabGroup.ActiveToggles();

    public void AddQuestToActiveListView(Quest quest, UnityAction<bool> onClicked)
        => m_ActiveQuestListView.AddElement(quest, onClicked);

    public void RemoveQuestFromActiveListView(Quest quest)
        => m_ActiveQuestListView.RemoveElement(quest);

    public void AddQuestToCompletedListView(Quest quest, UnityAction<bool> onClicked)
        => m_CompletedQuestListView.AddElement(quest, onClicked);
}
