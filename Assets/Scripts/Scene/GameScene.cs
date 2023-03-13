using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SingletomManager<GameScene>
{
    [SerializeField]
    [Header("각각 몬스터 스폰 위치 ( 테스트 )")]
    Transform m_MutantSpawnPoint, m_WarrockSpawnPoint, m_MawSpawnPoint;

    [Header("현재 진행 퀘스트")]
    public Quest m_CurQuest;
    [SerializeField]
    private Category m_Category;

    void Start()
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

        UpdateCheckCurrentQuest();
    }

    public void UpdateCheckCurrentQuest()
    {
        foreach (var quest in QuestSystem.Instance.p_ActiveQuests)
        {
            if (quest.p_Category == m_Category)
                m_CurQuest = quest;
        }
    }
}
