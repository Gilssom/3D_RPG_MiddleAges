using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AchievementView : UI_Popup
{
    [SerializeField]
    private RectTransform m_AchievementGroup;
    [SerializeField]
    private UI_AchievementDatailView m_AchievementDetailViewPrefab;

    public override void Init()
    {
        base.Init();

        var questSystem = QuestSystem.Instance;
        CreateDetailViews(questSystem.p_ActiveAchievements);
        CreateDetailViews(questSystem.p_CompletedAchievements);

        gameObject.SetActive(false);
    }

    public void OpenAchievement()
    {
        gameObject.SetActive(true);
    }

    public void CloseAchievement()
    {
        gameObject.SetActive(false);
    }

    private void CreateDetailViews(IReadOnlyList<Quest> achievements)
    {
        foreach (var achievement in achievements)
            Instantiate(m_AchievementDetailViewPrefab, m_AchievementGroup).Setup(achievement);
    }
}
