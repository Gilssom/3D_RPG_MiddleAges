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
        // ���� ���� 1��° ���� ���Ǹ� ���� �з��̹Ƿ� i = 1���� ����
        for (int i = 1; i < rows.Length; i++)
        {
            // A, B, C���� �ɰ��� �迭�� ����
            string[] rowValues = rows[i].Split(new char[] { ',' });

            // ��ȿ�� �̺�Ʈ �̸��� ���ö����� �ݺ�
            if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end") 
                continue;

            List<TalkData> talkDataList = new List<TalkData>();
            string eventName = rowValues[0];

            while (rowValues[0].Trim() != "end") // talkDataList �ϳ��� ����� �ݺ���
            {
                // ĳ���Ͱ� �ѹ��� ġ�� ����� ���̸� �𸣹Ƿ� ����Ʈ�� ����
                List<string> contextList = new List<string>();

                TalkData talkData = new TalkData();
                talkData.m_Name = rowValues[1]; // ĳ���� �̸��� �ִ� B��

                do // talkData �ϳ��� ����� �ݺ���
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

    /*public Dialogue[] Parse(string csvFileName)
    {
        List<Dialogue> dialoguesList = new List<Dialogue>(); // ��� ����Ʈ ����
        TextAsset csvData = ResourcesManager.Instance.Load<TextAsset>(csvFileName);

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); // , �� �������� ����

            Dialogue dialogue = new Dialogue(); // ��� ����Ʈ ����

            dialogue.m_Name = row[1];

            List<string> contextList = new List<string>();

            do
            {
                contextList.Add(row[2]);

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }

            } while (row[0].ToString() == "");

            dialogue.m_Contexts = contextList.ToArray();

            dialoguesList.Add(dialogue);
        }

        return dialoguesList.ToArray();
    }*/
}
