using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class QuestCompletionNotifier : MonoBehaviour
{
    [SerializeField]
    private string m_TitleDescription;

    [SerializeField]
    private TextMeshProUGUI m_TitleText;
    [SerializeField]
    private TextMeshProUGUI m_RewardText;
    [SerializeField]
    private float m_ShowTime = 3f;

    private Queue<Quest> m_ReserveQuests = new Queue<Quest>();
    private StringBuilder m_StringBuilder = new StringBuilder();

    private void Start()
    {
        var questSystem = QuestSystem.Instance;
        questSystem.onQuestCompleted += Notify;
        questSystem.onAchievementCompleted += Notify;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;
        if (questSystem != null)
        {
            questSystem.onQuestCompleted -= Notify;
            questSystem.onAchievementCompleted -= Notify;
        }
    }

    private void Notify(Quest quest)
    {
        m_ReserveQuests.Enqueue(quest);

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowNotice());
        }
    }

    private IEnumerator ShowNotice()
    {
        var waitSeconds = new WaitForSeconds(m_ShowTime);

        while (m_ReserveQuests.Count > 0)
        {
            var quest = m_ReserveQuests.Dequeue();

            m_TitleText.text = m_TitleDescription.Replace("%{dn}", quest.p_DisPlayName);

            foreach (var reward in quest.p_Rewards)
            {
                m_StringBuilder.Append(reward.p_Description);

                m_StringBuilder.Append(" ");

                m_StringBuilder.Append(reward.p_Quantity);

                m_StringBuilder.Append(" ");

            }
            m_RewardText.text = m_StringBuilder.ToString();

            m_StringBuilder.Clear();

            yield return waitSeconds;
        }

        gameObject.SetActive(false);
    }
}
