using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystemSaveTest : MonoBehaviour
{
    [SerializeField]
    private Quest m_Quest;
    [SerializeField]
    private Category m_Category;
    [SerializeField]
    private TaskTarget m_Target;

    void Start()
    {
        var questSystem = QuestSystem.Instance;

        if (questSystem.p_ActiveQuests.Count == 0)
        {
            Debug.Log("등록");
            var newQuest = questSystem.Register(m_Quest);
        }
        else
        {
            questSystem.onQuestCompleted += (quest) =>
            {
                Debug.Log("완료");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            };
        }
    }

    public void Test()
    {;
        QuestSystem.Instance.ReceiveReport(m_Category, m_Target, 1);
    }
}
