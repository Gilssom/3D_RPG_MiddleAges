using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestDetailView : MonoBehaviour
{
    [SerializeField]
    private GameObject m_DisplayGroup;
    [SerializeField]
    protected Button m_CancelButton;

    [Header("퀘스트 정보")]
    [SerializeField]
    private TextMeshProUGUI m_Title;
    [SerializeField]
    private TextMeshProUGUI m_Description;

    // Pool Count :: Reward Text 와 Task Text 를 Pooling
    [Header("수행 내역 정보")]
    [SerializeField]
    private RectTransform m_TaskDescriptionGroup;
    [SerializeField]
    private UI_TaskDescription m_TaskDescriptorPrefab;
    [SerializeField]
    private int m_TaskDescriptorPoolCount;

    [Header("퀘스트 보상 정보")]
    [SerializeField]
    private RectTransform m_RewardDescriptionGroup;
    [SerializeField]
    private UI_Inven_Slot m_RewardDescriptionPrefab;
    [SerializeField]
    private int m_RewardDescriptionPoolCount;

    private List<UI_TaskDescription> m_TaskDescriptorPool;
    private List<UI_Inven_Slot> m_RewardDescriptorPool;

    public Quest m_Target { get; private set; }

    private void Awake()
    {
        m_TaskDescriptorPool = CreatePool(m_TaskDescriptorPrefab, m_TaskDescriptorPoolCount, m_TaskDescriptionGroup);
        m_RewardDescriptorPool = CreatePool(m_RewardDescriptionPrefab, m_RewardDescriptionPoolCount, m_RewardDescriptionGroup);
        m_DisplayGroup.SetActive(false);
    }

    private void Start()
    {
        m_CancelButton.onClick.AddListener(CancelQuest);
    }

    private List<T> CreatePool<T>(T prefab, int count, RectTransform parent) where T : MonoBehaviour
    {
        var pool = new List<T>(count);
        for (int i = 0; i < count; i++)
            pool.Add(Instantiate(prefab, parent));
       
        return pool;
    }

    private void CancelQuest()
    {
        if (m_Target.p_IsCancelable)
            m_Target.Cancel();
    }

    public virtual void Show(Quest quest)
    {
        m_DisplayGroup.SetActive(true);
        m_Target = quest;

        m_Title.text = quest.p_DisPlayName;
        m_Description.text = quest.p_Description;

        int taskIndex = 0;
        foreach (var taskGroup in quest.p_TaskGroups)
        {
            foreach (var task in taskGroup.p_Tasks)
            {
                var poolObject = m_TaskDescriptorPool[taskIndex++];
                poolObject.gameObject.SetActive(true);

                if (taskGroup.p_IsComplete)
                    poolObject.UpdateTextUsingStrikeThrough(task);
                else if (taskGroup == quest.p_CurrentTaskGroup)
                    poolObject.UpdateText(task);
                else
                    poolObject.UpdateText("→ ??????????");
            }
        }

        for (int i = taskIndex; i < m_TaskDescriptorPool.Count; i++)
            m_TaskDescriptorPool[i].gameObject.SetActive(false);

        var rewards = quest.p_Rewards;
        var rewardCount = rewards.Count;

        for (int i = 0; i < m_RewardDescriptionPoolCount; i++)
        {
            var poolObject = m_RewardDescriptorPool[i];

            if (i < rewardCount)
            {
                var reward = rewards[i];

                if (reward.p_Item != null)
                {
                    poolObject.m_Item = reward.p_Item;
                    poolObject.m_SlotImage.sprite = reward.p_Item.m_ItemImage;
                    poolObject.m_SlotCount.text = reward.p_Quantity.ToString();
                }
                else
                {
                    poolObject.m_Item = null;
                    poolObject.m_SlotImage.sprite = reward.p_Icon;
                    poolObject.m_SlotCount.text = reward.p_Quantity.ToString();
                }

                poolObject.gameObject.SetActive(true);
            }
            else
                poolObject.gameObject.SetActive(false);
        }

        m_CancelButton.gameObject.SetActive(quest.p_IsCancelable && !quest.p_IsComplete);
    }

    public void Hide()
    {
        m_Target = null;
        m_DisplayGroup.SetActive(false);
        m_CancelButton.gameObject.SetActive(false);
    }
}
