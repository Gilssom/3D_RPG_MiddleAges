using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskGroupState
{
    Inactive,
    Running,
    Complete,
}

[System.Serializable]
public class TaskGroup
{
    [SerializeField]
    private Task[] m_Tasks;

    public IReadOnlyList<Task> p_Tasks => m_Tasks;

    // TaskGroup �� ����
    public Quest m_Owner { get; private set; }

    public bool p_IsAllTaskComplete => m_Tasks.All(x => x.IsComplete);
    public bool p_IsComplete => m_State == TaskGroupState.Complete;

    public TaskGroupState m_State { get; private set; }

    public void Setup(Quest owner)
    {
        m_Owner = owner;
        foreach (var task in m_Tasks)
            task.Setup(owner);
    }

    public void Start()
    {
        m_State = TaskGroupState.Running;
        foreach (var task in m_Tasks)
            task.Start();
    }

    public void End()
    {
        m_State = TaskGroupState.Complete;
        foreach (var task in m_Tasks)
            task.End();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        foreach (var task in m_Tasks)
        {
            if (task.IsTarget(category, target))
                task.ReceiveReport(successCount);
        }
    }

    public void Complete()
    {
        if (p_IsComplete)
            return;

        m_State = TaskGroupState.Complete;

        foreach (var task in m_Tasks)
        {
            if (!task.IsComplete)
                task.Complete();
        }
    }
}
