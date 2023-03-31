using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SingletomManager<GameScene>
{
    [SerializeField]
    [Header("플레이어 스폰 위치")]
    Transform m_PlayerSpawnPoint;

    [SerializeField]
    [Header("각각 몬스터 스폰 위치")]
    Transform m_MutantSpawnPoint, m_WarrockSpawnPoint, m_MawSpawnPoint;

    [Header("현재 진행 퀘스트")]
    public Quest m_CurQuest;
    [SerializeField]
    private Category m_Category;

    [Header("메인 퀘스트 진행도")]
    public Quest[] m_MainQuest;
    public int m_MainQuestIndex;

    [Header("Npc 관리")]
    public NpcInteraction[] m_Npc;

    [Header("Sound 관리")]
    public AudioClip[] m_Clip;

    public UI_Player_GUI m_PlayerGUI { get; private set; }
    public UI_LevelUp m_LevelUpUI { get; private set; }
    public UI_VillageName m_VillageNameUI { get; private set; }
    public UI_Minimap m_MinimapUI { get; private set; }
    public FadeInOutManager m_BloodScreenUI { get; private set; }
    public FadeInOutManager m_BlackScreenUI { get; private set; }

    bool isOpenSetting = false;

    void Awake()
    {
        SceneManagerEx.Instance.m_BlackScreenUI.StartFadeOut(3f);

        GameObject player = GameManager.Instance.Spwan(Defines.WorldObject.Player, "Player/Player", m_PlayerSpawnPoint);
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

        m_PlayerGUI = UIManager.Instance.ShowSceneUI<UI_Player_GUI>();
        m_LevelUpUI = UIManager.Instance.ShowSceneUI<UI_LevelUp>();
        m_VillageNameUI = UIManager.Instance.ShowSceneUI<UI_VillageName>();
        m_MinimapUI = UIManager.Instance.ShowSceneUI<UI_Minimap>();
        m_BloodScreenUI = ResourcesManager.Instance.Instantiate($"UI/Scene/UI_BloodScreen").GetComponent<FadeInOutManager>();

        StartQuest();
        UpdateCheckCurrentQuest();
        UpdateNpcState();

        SoundManager.Instance.Play("Bgm/Village", Defines.Sound.Bgm);
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

    public void UpdateAreaField()
    {
        m_VillageNameUI.VillageNameSetText();
        m_MinimapUI.MinimapNameSetText();
    }

    public void SoundSetting()
    {
        if (!isOpenSetting)
        {
            UIManager.Instance.ShowPopupUI<UI_Setting>();
            isOpenSetting = true;
            return;
        }

        UIManager.Instance.ClosePopupUI();
        isOpenSetting = false;
    }
}
