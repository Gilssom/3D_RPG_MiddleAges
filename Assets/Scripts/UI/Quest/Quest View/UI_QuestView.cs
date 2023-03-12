using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestView : UI_Popup
{
    [SerializeField]
    private UI_QuestListViewCtrl m_QuestListViewCtrl;
    [SerializeField]
    private UI_QuestDetailView m_QuestDetailView;

    public override void Init()
    {
        base.Init();

        var questSystem = QuestSystem.Instance;

        foreach (var quest in questSystem.p_ActiveQuests)
            AddQuestToActiveListView(quest);

        foreach (var quest in questSystem.p_CompletedQuests)
            AddQuestToActiveListView(quest);

        // Event 구조를 잘 만들어 놓으면 개발이 정말 편해진다.
        questSystem.onQuestRegistered += AddQuestToActiveListView;
        questSystem.onQuestCompleted += RemoveQuestFromActiveListView;
        questSystem.onQuestCompleted += AddQuestToCompletedListView;
        questSystem.onQuestCompleted += HideDetailIfQuestCanceled;
        questSystem.onQuestCanceled += HideDetailIfQuestCanceled;
        questSystem.onQuestCanceled += RemoveQuestFromActiveListView;

        foreach (var tab in m_QuestListViewCtrl.m_Tabs)
            tab.onValueChanged.AddListener(HideDetail);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;

        if (questSystem)
        {
            questSystem.onQuestRegistered -= AddQuestToActiveListView;
            questSystem.onQuestCompleted -= RemoveQuestFromActiveListView;
            questSystem.onQuestCompleted -= AddQuestToCompletedListView;
            questSystem.onQuestCompleted -= HideDetailIfQuestCanceled;
            questSystem.onQuestCanceled -= HideDetailIfQuestCanceled;
            questSystem.onQuestCanceled -= RemoveQuestFromActiveListView;
        }
    }

    private void OnEnable()
    {
        if (m_QuestDetailView.m_Target != null)
            m_QuestDetailView.Show(m_QuestDetailView.m_Target);
    }

    public void OpenQuest()
    {
        gameObject.SetActive(true);
    }

    public void CloseQuest()
    {
        gameObject.SetActive(false);
    }

    private void ShowDetail(bool isOn, Quest quest)
    {
        if (isOn)
            m_QuestDetailView.Show(quest);
    }

    private void HideDetail(bool isOn)
    {
        m_QuestDetailView.Hide();
    }

    private void AddQuestToActiveListView(Quest quest)
        => m_QuestListViewCtrl.AddQuestToActiveListView(quest, isOn => ShowDetail(isOn, quest));

    private void AddQuestToCompletedListView(Quest quest)
        => m_QuestListViewCtrl.AddQuestToCompletedListView(quest, isOn => ShowDetail(isOn, quest));

    private void HideDetailIfQuestCanceled(Quest quest)
    {
        if (m_QuestDetailView.m_Target == quest)
            m_QuestDetailView.Hide();
    }

    private void RemoveQuestFromActiveListView(Quest quest)
    {
        m_QuestListViewCtrl.RemoveQuestFromActiveListView(quest);

        if (m_QuestDetailView.m_Target == quest)
            m_QuestDetailView.Hide();
    }
}
