using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR // �����Ϳ����� ����� ���ӽ����̽�
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Quest/QuestDatabase")]
public class QuestDataBase : ScriptableObject
{
    [SerializeField]
    private List<Quest> m_Quests;

    public IReadOnlyList<Quest> p_Quests => m_Quests;

    public Quest FindQuestBy(string codeName) => m_Quests.FirstOrDefault(x => x.p_CodeName == codeName);

    /// <summary>
    /// Resources �� Quest / Achievement Data Base SO �� ������ �� ��3�� ��ư�� ����
    /// �ش� Ÿ�Կ� �´� Find �Լ��� �����ϸ� �׿� �´� Ÿ���� SO �� ������ �ȴ�.
    /// </summary>
#if UNITY_EDITOR
    [ContextMenu("FindQuests")]
    private void FindQuests()
    {
        FindQuestsBy<Quest>();
    }

    [ContextMenu("FindAchievements")]
    private void FindAchievement()
    {
        FindQuestsBy<Achievement>();
    }


    private void FindQuestsBy<T>() where T : Quest
    {
        m_Quests = new List<Quest>();

        // FindAsset :: Asset �������� Filter �� �´� Asset �� GUID �� �������� �Լ��̴�.
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (quest.GetType() == typeof(T))
                m_Quests.Add(quest);

            // SetDirty :: QuestDataBase ��ü�� ���� Serialize ������ ��ȭ�� ��������
            // Asset �� ������ �� ��ݿ��� �϶�� �ǹ�
            EditorUtility.SetDirty(this);

            // Editor �� ���� Ű�� �ʱ�ȭ�� �� �� ������ ������ �ϴ� �ǹ�
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
