using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR // 에디터에서만 사용할 네임스페이스
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
    /// Resources 의 Quest / Achievement Data Base SO 에 오른쪽 위 점3개 버튼을 눌러
    /// 해당 타입에 맞는 Find 함수를 실행하면 그에 맞는 타입의 SO 가 들어오게 된다.
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

        // FindAsset :: Asset 폴더에서 Filter 에 맞는 Asset 의 GUID 를 가져오는 함수이다.
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (quest.GetType() == typeof(T))
                m_Quests.Add(quest);

            // SetDirty :: QuestDataBase 객체가 가진 Serialize 변수가 변화가 생겼으니
            // Asset 을 저장할 때 재반영을 하라는 의미
            EditorUtility.SetDirty(this);

            // Editor 를 껐다 키면 초기화가 될 수 있으니 저장을 하는 의미
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
