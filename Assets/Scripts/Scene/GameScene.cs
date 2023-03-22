using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SingletomManager<GameScene>
{
    [SerializeField]
    [Header("���� ���� ���� ��ġ ( �׽�Ʈ )")]
    Transform m_MutantSpawnPoint, m_WarrockSpawnPoint, m_MawSpawnPoint;

    [Header("���� ���� ����Ʈ")]
    public Quest m_CurQuest;
    [SerializeField]
    private Category m_Category;

    [Header("���� ����Ʈ ���൵")]
    public Quest[] m_MainQuest;
    public int m_MainQuestIndex;

    [Header("Npc ����")]
    public NpcInteraction[] m_Npc;

    void Awake()
    {
        GameObject player = GameManager.Instance.Spwan(Defines.WorldObject.Player, "Player/Player");
        Camera.main.gameObject.GetOrAddComponet<CameraManager>().SetPlayer(player);

        GameObject go = new GameObject { name = "SpawningPool" };
        MutantSpawn Mutantpool = go.GetOrAddComponet<MutantSpawn>();
        WarrockSpawn Warrockpool = go.GetOrAddComponet<WarrockSpawn>();
        MawSpawn Mawpool = go.GetOrAddComponet<MawSpawn>();
        Mutantpool.SetKeepMonsterCount(5);
        Mutantpool.SetPosition(m_MutantSpawnPoint.position);
        Warrockpool.SetKeepMonsterCount(3);
        Warrockpool.SetPosition(m_WarrockSpawnPoint.position);
        Mawpool.SetKeepMonsterCount(2);
        Mawpool.SetPosition(m_MawSpawnPoint.position);

        UIManager.Instance.ShowSceneUI<UI_Player_GUI>();

        StartQuest();
        UpdateCheckCurrentQuest();
        UpdateNpcState();
    }

    // Main Quest State Update
    public void StartQuest()
    {
        foreach (var quest in m_MainQuest)
        {
            Debug.Log($"{quest} / {quest.p_IsAcceptable} / {!QuestSystem.Instance.ContainsInCompleteQuests(quest)}");
            if (quest.p_IsAcceptable && !QuestSystem.Instance.ContainsInCompleteQuests(quest))
            {
                QuestSystem.Instance.Register(quest);
                UpdateCheckCurrentQuest();
                break;
            }
        }
    }

    public void UpdateCheckCurrentQuest()
    {
        foreach (var quest in QuestSystem.Instance.p_ActiveQuests)
        {
            if (quest.p_Category == m_Category)
                m_CurQuest = quest;
        }
    }

    // Sub Quest State Update
    public void UpdateNpcState()
    {
        for (int i = 0; i < m_Npc.Length; i++)
        {
            m_Npc[i].UpdateQuestState();
        }
    }
}
