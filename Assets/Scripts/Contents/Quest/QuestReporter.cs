using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestReporter : MonoBehaviour
{
    [SerializeField]
    private Category m_Category;
    [SerializeField]
    private TaskTarget m_Target;
    [SerializeField]
    private int m_SuccessCount;
    [SerializeField]
    private string[] m_ColliderTags;

    private void OnTriggerEnter(Collider other)
    {
        ReportIfPassCondition(other);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReportIfPassCondition(collision);
    }

    public void Report()
    {
        QuestSystem.Instance.ReceiveReport(m_Category, m_Target, m_SuccessCount);
    }

    private void ReportIfPassCondition(Component other)
    {
        if (m_ColliderTags.Any(x => other.CompareTag(x)))
            Report();
    }
}
