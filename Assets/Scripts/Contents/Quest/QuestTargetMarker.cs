using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestTargetMarker : MonoBehaviour
{
    [SerializeField]
    private TaskTarget m_Target;
    [SerializeField]
    private MarkerMaterialData[] m_MarkerMaterialDatas;

    public bool isQuestTarget;

    private Dictionary<Quest, Task> m_TargetTasksByQuest = new Dictionary<Quest, Task>();
    private Transform m_CameraTransform;
    //private Renderer m_Renderer;

    private int m_CurrentRunningTargetTaskCount;

    private void Awake()
    {
        m_CameraTransform = Camera.main.transform;
        //m_Renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        QuestSystem.Instance.onQuestRegistered += TryAddTargetQuest;
        foreach (var quest in QuestSystem.Instance.p_ActiveQuests)
            TryAddTargetQuest(quest);

        //gameObject.SetActive(false);
    }

    private void Update()
    {
        // ��Ŀ�� �÷��̾ �ٶ󺸰�
        var rotation = Quaternion.LookRotation((m_CameraTransform.position - transform.position).normalized);
        transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180f, 0f);
    }

    private void OnDestroy()
    {
        // event ����
        QuestSystem.Instance.onQuestRegistered -= TryAddTargetQuest;
        foreach (var keyPair in m_TargetTasksByQuest)
        {
            var quest = keyPair.Key;
            var task = keyPair.Value;

            quest.onNewTaskGroup -= UpdateTargetTask;
            quest.onCompleted -= RemoveTargetQuest;
            task.onStateChanged -= UpdateRunningTargetTaskCount;
        }
    }

    // ��ϵ� Quest �� Ȯ���Ͽ� Target �� ��� �������ִ� �Լ�
    private void TryAddTargetQuest(Quest quest)
    {
        if (m_Target != null && quest.ContainsTarget(m_Target))
        {
            quest.onNewTaskGroup += UpdateTargetTask;
            quest.onCompleted += RemoveTargetQuest;

            UpdateTargetTask(quest, quest.p_CurrentTaskGroup);
        }
    }

    // �������� Task �� ��ü�ϴ� �Լ�
    private void UpdateTargetTask(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
    {
        m_TargetTasksByQuest.Remove(quest);

        var task = currentTaskGroup.FindTaskByTarget(m_Target);
        if (task != null)
        {
            m_TargetTasksByQuest[quest] = task;
            task.onStateChanged += UpdateRunningTargetTaskCount;

            UpdateRunningTargetTaskCount(task, task.State);
        }
    }

    // Quest �Ϸ� �� �ش� Target ���� �����ִ� �Լ�
    private void RemoveTargetQuest(Quest quest) => m_TargetTasksByQuest.Remove(quest);

    // Marker �� ų�� ��ų��
    private void UpdateRunningTargetTaskCount(Task task, TaskState currentState, TaskState prevState = TaskState.Inactive)
    {
        if (currentState == TaskState.Running)
        {
            //m_Renderer.material = m_MarkerMaterialDatas.First(x => x.m_Category == task.p_Category).m_MarkerMaterial;
            Instantiate(m_MarkerMaterialDatas.First(x => x.m_Category == task.p_Category).m_MarkerPrefab, transform);
            m_CurrentRunningTargetTaskCount++;
        }
        else
            m_CurrentRunningTargetTaskCount--;

        gameObject.SetActive(m_CurrentRunningTargetTaskCount != 0);
        isQuestTarget = (m_CurrentRunningTargetTaskCount != 0);
    }

    [System.Serializable]
    private struct MarkerMaterialData
    {
        public Category m_Category;
        //public Material m_MarkerMaterial;
        public GameObject m_MarkerPrefab;
    }
}
