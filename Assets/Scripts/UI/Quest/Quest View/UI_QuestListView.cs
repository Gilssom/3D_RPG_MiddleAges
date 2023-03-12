using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UI_QuestListView : MonoBehaviour
{
    // Element : ���� ���� Quest���� �̸��� Text Button ���� �����ִ� ��
    [SerializeField]
    private TextMeshProUGUI m_ElementTextPrefab;

    private Dictionary<Quest, GameObject> m_ElementsByQuest = new Dictionary<Quest, GameObject>();
    private ToggleGroup m_ToggleGroup;

    private void Awake()
    {
        m_ToggleGroup = GetComponent<ToggleGroup>();
    }

    public void AddElement(Quest quest, UnityAction<bool> onClicked)
    {
        var element = Instantiate(m_ElementTextPrefab, transform);
        element.text = quest.p_DisPlayName;

        var toggle = element.GetComponent<Toggle>();
        toggle.group = m_ToggleGroup;
        toggle.onValueChanged.AddListener(onClicked);

        m_ElementsByQuest.Add(quest, element.gameObject);
    }

    public void RemoveElement(Quest quest)
    {
        Destroy(m_ElementsByQuest[quest]);
        m_ElementsByQuest.Remove(quest);
    }
}
