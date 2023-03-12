using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_AchievementDatailView : MonoBehaviour
{
    [SerializeField]
    private Image m_AchievementIcon;
    [SerializeField]
    private TextMeshProUGUI m_TitleText;
    [SerializeField]
    private TextMeshProUGUI m_Description;
    [SerializeField]
    private Image m_RewardIcon;
    [SerializeField]
    private TextMeshProUGUI m_RewardText;
    [SerializeField]
    private GameObject m_CompletionScreen;

    private Quest m_Target;

    private void OnDestroy()
    {
        if (m_Target != null)
        {
            m_Target.onTaskSuccessChanged -= UpdateDescription;
            m_Target.onCompleted -= ShowCompletionScreen;
        }
    }

    public void Setup(Quest achievement)
    {
        m_Target = achievement;

        m_AchievementIcon.sprite = achievement.p_Icon;
        m_TitleText.text = achievement.p_DisPlayName;

        var task = achievement.p_CurrentTaskGroup.p_Tasks[0];
        m_Description.text = BuildTaskDescrtion(task);

        var reward = achievement.p_Rewards[0];
        m_RewardIcon.sprite = reward.p_Icon;
        m_RewardText.text = $"{reward.p_Description} +{reward.p_Quantity}";

        if (achievement.p_IsComplete)
            m_CompletionScreen.SetActive(true);
        else
        {
            m_CompletionScreen.SetActive(false);
            achievement.onTaskSuccessChanged += UpdateDescription;
            achievement.onCompleted += ShowCompletionScreen;
        }
    }

    private void UpdateDescription(Quest achievement, Task task, int currentSuccess, int prevSuccess)
        => m_Description.text = BuildTaskDescrtion(task);

    private void ShowCompletionScreen(Quest achievement)
        => m_CompletionScreen.SetActive(true);

    private string BuildTaskDescrtion(Task task) => $"{task.p_Description} {task.p_CurrentSucceses}/{task.p_NeedSuccessToComplete}";
}
