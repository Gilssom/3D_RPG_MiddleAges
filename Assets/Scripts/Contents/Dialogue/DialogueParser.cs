using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    [SerializeField]
    private string m_csvName;
    private TextAsset m_csvFile = null;

    string csvText;
    string[] rows;

    private void Awake()
    {
        m_csvFile = ResourcesManager.Instance.Load<TextAsset>(m_csvName);

        csvText = m_csvFile.text.Substring(0, m_csvFile.text.Length - 1);

        rows = csvText.Split(new char[] { '\n' });

        SetTalkDictionary();
        SetDebugTalkData();
    }

    public static Dictionary<string, TalkData[]> DialogueDictionary = new Dictionary<string, TalkData[]>();

    [SerializeField]
    List<DebugTalkData> DebugTalkDataList = new List<DebugTalkData>();

    public void SetTalkDictionary()
    {
        for (int i = 1; i < rows.Length; i++)
        {
            string[] rowValues = rows[i].Split(new char[] { ',' });

            if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end") 
                continue;

            List<TalkData> talkDataList = new List<TalkData>();
            string eventName = rowValues[0];

            while (rowValues[0].Trim() != "end")
            {
                List<string> contextList = new List<string>();

                TalkData talkData = new TalkData();
                talkData.m_Name = rowValues[1];

                do
                {
                    contextList.Add(rowValues[2].ToString());
                    if (++i < rows.Length)
                        rowValues = rows[i].Split(new char[] { ',' });
                    else break;
                } while (rowValues[1] == "" && rowValues[0] != "end");

                talkData.m_Contexts = contextList.ToArray();
                talkDataList.Add(talkData);
            }

            DialogueDictionary.Add(eventName, talkDataList.ToArray());
        }
    }

    public static TalkData[] GetDialogue(string eventName)
    {
        // Ű�� ��Ī�Ǵ� ���� ������ true ������ false
        if (DialogueDictionary.ContainsKey(eventName))
            return DialogueDictionary[eventName];
        else
        {
            // ��� ����ϰ� null ��ȯ
            Debug.LogWarning("ã�� �� ���� �̺�Ʈ �̸� : " + eventName);
            return null;
        }
    }

    void SetDebugTalkData()
    {
        // ��ųʸ��� Ű ������ ���� ����Ʈ
        List<string> eventNames =
                    new List<string>(DialogueDictionary.Keys);
        // ��ųʸ��� ��� ������ ���� ����Ʈ
        List<TalkData[]> talkDatasList =
                    new List<TalkData[]>(DialogueDictionary.Values);

        // ��ųʸ��� ũ�⸸ŭ �߰�
        for (int i = 0; i < eventNames.Count; i++)
        {
            DebugTalkData debugTalk =
                new DebugTalkData(eventNames[i], talkDatasList[i]);

            DebugTalkDataList.Add(debugTalk);
        }
    }
}
