using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_QuestTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_QuestTitleText;
    [SerializeField]
    private UI_TaskDescription m_TaskDescriptionPrefab;

    private Dictionary<Task, UI_TaskDescription> m_TaskDescriptorsByTask = new Dictionary<Task, UI_TaskDescription>();

    private Quest m_TargetQuest;

    private void OnDestroy()
    {
        if (m_TargetQuest != null)
        {
            m_TargetQuest.onNewTaskGroup -= UpdateTaskDescriptors;
            m_TargetQuest.onCompleted -= DestroySelf;
        }

        foreach (var tuple in m_TaskDescriptorsByTask)
        {
            var task = tuple.Key;
            task.onSuccessChanged -= UpdateText;
        }
    }

    public void Setup(Quest targetQuest, Color titleColor)
    {
        this.m_TargetQuest = targetQuest;

        m_QuestTitleText.text = targetQuest.p_Category == null ?
            targetQuest.p_DisPlayName : $"[{targetQuest.p_Category.p_DisPlayName}] {targetQuest.p_DisPlayName}";

        m_QuestTitleText.color = titleColor;

        // 항상 등록한 event 를 제거해줘야 한다.
        targetQuest.onNewTaskGroup += UpdateTaskDescriptors;
        targetQuest.onCompleted += DestroySelf;

        var taskGroups = targetQuest.p_TaskGroups;
        UpdateTaskDescriptors(targetQuest, taskGroups[0]);

        if (taskGroups[0] != targetQuest.p_CurrentTaskGroup)
        {
            for (int i = 1; i < taskGroups.Count; i++)
            {
                var taskGroup = taskGroups[i];
                UpdateTaskDescriptors(targetQuest, taskGroup, taskGroups[i - 1]);

                if (taskGroup == targetQuest.p_CurrentTaskGroup)
                    break;
            }
        }
    }

    private void UpdateTaskDescriptors(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
    {
        foreach (var task in currentTaskGroup.p_Tasks)
        {
            var taskDescriptor = Instantiate(m_TaskDescriptionPrefab, transform);
            taskDescriptor.UpdateText(task);
            task.onSuccessChanged += UpdateText;

            m_TaskDescriptorsByTask.Add(task, taskDescriptor);
        }

        if (prevTaskGroup != null)
        {
            foreach (var task in prevTaskGroup.p_Tasks)
            {
                var taskDescriptor = m_TaskDescriptorsByTask[task];
                taskDescriptor.UpdateTextUsingStrikeThrough(task);
            }
        }
    }

    private void UpdateText(Task task, int currentSeccess, int prevSuccess)
    {
        m_TaskDescriptorsByTask[task].UpdateText(task);
    }

    private void DestroySelf(Quest quest)
    {
        Destroy(gameObject);
    }
}
