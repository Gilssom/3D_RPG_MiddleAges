using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystemTest : MonoBehaviour
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

        questSystem.onQuestRegistered += (m_Quest) =>
        {
            print($"새로운 퀘스트 :{m_Quest.p_CodeName} 등록!");
            print($"활성화 퀘스트 개수 :{questSystem.p_ActiveQuests.Count}");
        };

        questSystem.onQuestCompleted += (m_Quest) =>
        {
            print($"완료된 퀘스트 :{m_Quest.p_CodeName}");
            print($"완료된 퀘스트 개수 :{questSystem.p_CompletedQuests.Count}");
        };

        var newQuest = questSystem.Register(m_Quest);
        newQuest.onTaskSuccessChanged += (m_Quest, task, currentSuccess, prevSuccess) =>
        {
            print($"퀘스트 : {m_Quest.p_CodeName}, 수행 내역 : {task.p_CodeName}, 현재 성공 내역 : {currentSuccess}");
        };

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            QuestSystem.Instance.ReceiveReport(m_Category, m_Target, 1);
        }
    }
}
