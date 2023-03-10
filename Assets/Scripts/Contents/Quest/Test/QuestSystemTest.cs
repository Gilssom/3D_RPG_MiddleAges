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
            print($"���ο� ����Ʈ :{m_Quest.p_CodeName} ���!");
            print($"Ȱ��ȭ ����Ʈ ���� :{questSystem.p_ActiveQuests.Count}");
        };

        questSystem.onQuestCompleted += (m_Quest) =>
        {
            print($"�Ϸ�� ����Ʈ :{m_Quest.p_CodeName}");
            print($"�Ϸ�� ����Ʈ ���� :{questSystem.p_CompletedQuests.Count}");
        };

        var newQuest = questSystem.Register(m_Quest);
        newQuest.onTaskSuccessChanged += (m_Quest, task, currentSuccess, prevSuccess) =>
        {
            print($"����Ʈ : {m_Quest.p_CodeName}, ���� ���� : {task.p_CodeName}, ���� ���� ���� : {currentSuccess}");
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
