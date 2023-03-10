using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuestSystem : SingletomManager<QuestSystem>
{
    // Save ���� ����� Ű���� ( => ��Ÿ�� �ܺ����� ��ҿ� ���� ������ �����ϱ� ���� )
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

    // ������ ����ɶ� 
    private void OnApplicationQuit()
    {
        // ������ ������ ���׷� ���� ���ᰡ �Ǿ��� �� ������ �ȵ�
        // ��� ������ ������ �� ������ ���踦 �ϰ� ������ �ؾ���.
        isApplicationQuitting = true;
        Save();
    }

    // Quest ��� �Լ�
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

    // �ܺ� ���
    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(m_ActiveQuests, category, target, successCount);
        ReceiveReport(m_ActiveAchievements, category, target, successCount);
    }

    // OverLoad
    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.p_CodeName, target.Value, successCount);

    // ���� ���
    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        // ToArray �� List �纻�� ���� for���� ������ ����
        // => for���� ���ư��� ���� Quest �� Complete �� �Ǿ� ��Ͽ��� ���� �� �ֱ� ����
        // =/> ������ �����ٰ� ����� �ٲ�� �Ǹ� Error �� ���� ����
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

    // Save Data �� Json ���·� ��ȯ ��Ų ������,
    // Json Array �� �־��ְ� �ִ� �Լ�
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

    // datasToken :: ���� CreateSaveDatas �� ����� ������� SaveData �� ����Ǿ��ٰ�,
    //                Load �� �� �Լ��� ������ �ȴ�.
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

    // �ҷ��� Quest ���� ����� ���ְ�,
    // ����� Quest �� ����Ǿ� �ִ� Data �� �־��ش�.
    private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Register(quest);
        newQuest.LoadFrom(saveData);
    }

    // ����Ʈ | ���� �� ���� �߰����ִ� �Լ�
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

    #region #Callback => �̺�Ʈ �ֵ� ���α׷���
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
