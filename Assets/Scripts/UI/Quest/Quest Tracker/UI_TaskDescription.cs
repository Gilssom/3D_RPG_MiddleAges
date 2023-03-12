using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TaskDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Text;

    [SerializeField]
    private Color m_NormalColor;
    [SerializeField]
    private Color m_TaskCompleteColor;
    [SerializeField]
    private Color m_TaskSuccessCountColor;
    [SerializeField]
    private Color m_StrikeThroughColor;

    public void UpdateText(string text)
    {
        this.m_Text.fontStyle = FontStyles.Normal;
        this.m_Text.text = text;
    }

    public void UpdateText(Task task)
    {
        m_Text.fontStyle = FontStyles.Normal;

        if (task.IsComplete)
        {
            var colorCode = ColorUtility.ToHtmlStringRGB(m_TaskCompleteColor);
            m_Text.text = BuildText(task, colorCode, colorCode);
        }
        else
        {
            m_Text.text = BuildText(task, ColorUtility.ToHtmlStringRGB(m_NormalColor), ColorUtility.ToHtmlStringRGB(m_TaskSuccessCountColor));
        }
    }

    public void UpdateTextUsingStrikeThrough(Task task)
    {
        var colorCode = ColorUtility.ToHtmlStringRGB(m_StrikeThroughColor);
        m_Text.fontStyle = FontStyles.Strikethrough;
        m_Text.text = BuildText(task, colorCode, colorCode);
    }

    private string BuildText(Task task, string textColorCode, string successCountColorCode)
    {
        return $"<color=#{textColorCode}>→ {task.p_Description} <color=#{successCountColorCode}>{task.p_CurrentSucceses}</color>/{task.p_NeedSuccessToComplete}</color>";
    }
}
