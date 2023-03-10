using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuestSystem : SingletomManager<QuestSystem>
{
    // Save 에서 사용할 키값들 ( => 오타나 외부적인 요소에 대한 오류를 방지하기 위함 )
    #region # Save Path
    private const string k_SaveRootPath = "questSystem";
    private const string k_ActiveQuestsSavePath = "activeQuests";
    private const string k_CompletedQuestsSavePath = "completedQuests";
    private const string k_ActiveAchievementsSavePath = "activeAchievements";
    private const string k_CompletedAchievementsSavePath = "completedAchievements";
    #endregion

    #region # Events
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    #endregion

    private static bool isApplicationQuitting;

    private List<Quest> m_ActiveQuests = new List<Quest>();
    private List<Quest> m_CompletedQuests = new List<Quest>();

    private List<Quest> m_ActiveAchievements = new List<Quest>();
    private List<Quest> m_CompletedAchievements = new List<Quest>();

    private QuestDataBase m_QuestDataBase;
    private QuestDataBase m_AchievementDataBase;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;

    public event QuestRegisteredHandler onAchievementRegistered;
    public event QuestCompletedHandler onAchievementCompleted;

    public IReadOnlyList<Quest> p_ActiveQuests => m_ActiveQuests;
    public IReadOnlyList<Quest> p_CompletedQuests => m_CompletedQuests;

    public IReadOnlyList<Quest> p_ActiveAchievements => m_ActiveAchievements;
    public IReadOnlyList<Quest> p_CompletedAchievements => m_CompletedAchievements;

    private void Awake()
    {
        m_QuestDataBase = ResourcesManager.Instance.Load<QuestDataBase>("Quest Data Base");
        m_AchievementDataBase = ResourcesManager.Instance.Load<QuestDataBase>("Quest Data Base");

        if (!Load())
        {
            foreach (var achievement in m_AchievementDataBase.p_Quests)
                Register(achievement);
        }
    }

    // 게임이 종료될때 
    private void OnApplicationQuit()
    {
        // 게임이 오류나 버그로 강제 종료가 되었을 때 저장이 안됨
        // 어느 시점에 저장을 할 것인지 설계를 하고 적용을 해야함.
        isApplicationQuitting = true;
        Save();
    }

    // Quest 등록 함수
    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if (newQuest is Achievement)
        {
            newQuest.onCompleted += OnAchievementCompleted;

            m_ActiveAchievements.Add(newQuest);

            newQuest.OnRegister();
            onAchievementRegistered?.Invoke(newQuest);
        }
        else
        {
            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCanceled += OnQuestCanceled;

            m_ActiveQuests.Add(newQuest);

            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }

    // 외부 사용
    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(m_ActiveQuests, category, target, successCount);
        ReceiveReport(m_ActiveAchievements, category, target, successCount);
    }

    // OverLoad
    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.p_CodeName, target.Value, successCount);

    // 내부 사용
    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        // ToArray 로 List 사본을 만들어서 for문을 돌리는 이유
        // => for문이 돌아가는 와중 Quest 가 Complete 가 되어 목록에서 빠질 수 있기 때문
        // =/> 원본을 돌리다가 목록이 바뀌게 되면 Error 가 나기 때문
        foreach (var quest in quests.ToArray())
        {
            quest.ReceiveReport(category, target, successCount);
        }
    }

    public bool ContainsInActiveQuests(Quest quest) => m_ActiveQuests.Any(x => x.p_CodeName == quest.p_CodeName);
    public bool ContainsInCompleteQuests(Quest quest) => m_CompletedQuests.Any(x => x.p_CodeName == quest.p_CodeName);
    public bool ContainsInActiveAchievements(Quest quest) => m_ActiveAchievements.Any(x => x.p_CodeName == quest.p_CodeName);
    public bool ContainsInCompletedAchievements(Quest quest) => m_CompletedAchievements.Any(x => x.p_CodeName == quest.p_CodeName);

    #region # Save & Load
    private void Save()
    {
        var root = new JObject();
        root.Add(k_ActiveQuestsSavePath, CreateSaveDatas(m_ActiveQuests));
        root.Add(k_CompletedQuestsSavePath, CreateSaveDatas(m_CompletedQuests));
        root.Add(k_ActiveAchievementsSavePath, CreateSaveDatas(m_ActiveAchievements));
        root.Add(k_CompletedAchievementsSavePath, CreateSaveDatas(m_CompletedAchievements));

        PlayerPrefs.SetString(k_SaveRootPath, root.ToString());
        PlayerPrefs.Save();
    }

    private bool Load()
    {
        if (PlayerPrefs.HasKey(k_SaveRootPath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(k_SaveRootPath));

            LoadSaveDatas(root[k_ActiveQuestsSavePath], m_QuestDataBase, LoadActiveQuest);
            LoadSaveDatas(root[k_CompletedQuestsSavePath], m_QuestDataBase, LoadCompletedQuest);

            LoadSaveDatas(root[k_ActiveAchievementsSavePath], m_QuestDataBase, LoadActiveQuest);
            LoadSaveDatas(root[k_CompletedAchievementsSavePath], m_QuestDataBase, LoadActiveQuest);

            return true;
        }
        else
            return false;
    }

    // Save Data 를 Json 형태로 변환 시킨 다음에,
    // Json Array 에 넣어주고 있는 함수
    private JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        var saveDatas = new JArray();
        foreach (var quest in quests)
        {
            if(quest.p_IsSavable)
                saveDatas.Add(JObject.FromObject(quest.ToSaveData()));
        }
        return saveDatas;
    }

    // datasToken :: 위의 CreateSaveDatas 의 결과로 만들어진 SaveData 가 저장되었다가,
    //                Load 시 이 함수로 들어오게 된다.
    private void LoadSaveDatas(JToken datasToken, QuestDataBase dataBase, System.Action<QuestSaveData, Quest> onSuccess)
    {
        var datas = datasToken as JArray;
        foreach (var data in datas)
        {
            var saveData = data.ToObject<QuestSaveData>();
            var quest = dataBase.FindQuestBy(saveData.m_CodeName);
            onSuccess.Invoke(saveData, quest);
        }
    }

    // 불러온 Quest 들을 등록을 해주고,
    // 등록한 Quest 에 저장되어 있는 Data 를 넣어준다.
    private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Register(quest);
        newQuest.LoadFrom(saveData);
    }

    // 퀘스트 | 업적 에 따라 추가해주는 함수
    private void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadFrom(saveData);

        if (newQuest is Achievement)
            m_CompletedAchievements.Add(newQuest);
        else
            m_CompletedQuests.Add(newQuest);
    }
    #endregion

    #region #Callback => 이벤트 주도 프로그래밍
    private void OnQuestCompleted(Quest quest)
    {
        m_ActiveQuests.Remove(quest);
        m_CompletedQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        m_ActiveQuests.Remove(quest);
        onQuestCanceled?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }

    private void OnAchievementCompleted(Quest achievement)
    {
        m_ActiveAchievements.Remove(achievement);
        m_CompletedAchievements.Add(achievement);

        onAchievementCompleted?.Invoke(achievement);
    }
    #endregion
}
