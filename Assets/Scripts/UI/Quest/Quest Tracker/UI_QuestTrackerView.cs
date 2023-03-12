using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_QuestTrackerView : MonoBehaviour
{
    [SerializeField]
    private UI_QuestTracker m_QuestTrackerPrefab;
    [SerializeField]
    private CategoryColor[] m_CategoryColors;

    private void Start()
    {
        QuestSystem.Instance.onQuestRegistered += CreateQuestTracker;

        foreach (var quest in QuestSystem.Instance.p_ActiveQuests)
            CreateQuestTracker(quest);        
    }

    private void OnDestroy()
    {
        if (QuestSystem.Instance)
            QuestSystem.Instance.onQuestRegistered -= CreateQuestTracker;
    }

    private void CreateQuestTracker(Quest quest)
    {
        var categoryColor = m_CategoryColors.FirstOrDefault(x => x.m_Category == quest.p_Category);
        var color = categoryColor.m_Category == null ? Color.white : categoryColor.m_Color;
        Instantiate(m_QuestTrackerPrefab, transform).Setup(quest, color);
    }

    [System.Serializable]
    private struct CategoryColor
    {
        public Category m_Category;
        public Color m_Color;
    }
}
