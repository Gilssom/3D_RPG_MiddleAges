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
        // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        for (int i = 1; i < rows.Length; i++)
        {
            // A, B, C열을 쪼개서 배열에 담음
            string[] rowValues = rows[i].Split(new char[] { ',' });

            // 유효한 이벤트 이름이 나올때까지 반복
            if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end") 
                continue;

            List<TalkData> talkDataList = new List<TalkData>();
            string eventName = rowValues[0];

            while (rowValues[0].Trim() != "end") // talkDataList 하나를 만드는 반복문
            {
                // 캐릭터가 한번에 치는 대사의 길이를 모르므로 리스트로 선언
                List<string> contextList = new List<string>();

                TalkData talkData = new TalkData();
                talkData.m_Name = rowValues[1]; // 캐릭터 이름이 있는 B열

                do // talkData 하나를 만드는 반복문
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
        // 키에 매칭되는 값이 있으면 true 없으면 false
        if (DialogueDictionary.ContainsKey(eventName))
            return DialogueDictionary[eventName];
        else
        {
            // 경고 출력하고 null 반환
            Debug.LogWarning("찾을 수 없는 이벤트 이름 : " + eventName);
            return null;
        }
    }

    void SetDebugTalkData()
    {
        // 딕셔너리의 키 값들을 가진 리스트
        List<string> eventNames =
                    new List<string>(DialogueDictionary.Keys);
        // 딕셔너리의 밸류 값들을 가진 리스트
        List<TalkData[]> talkDatasList =
                    new List<TalkData[]>(DialogueDictionary.Values);

        // 딕셔너리의 크기만큼 추가
        for (int i = 0; i < eventNames.Count; i++)
        {
            DebugTalkData debugTalk =
                new DebugTalkData(eventNames[i], talkDatasList[i]);

            DebugTalkDataList.Add(debugTalk);
        }
    }

    /*public Dialogue[] Parse(string csvFileName)
    {
        List<Dialogue> dialoguesList = new List<Dialogue>(); // 대사 리스트 생성
        TextAsset csvData = ResourcesManager.Instance.Load<TextAsset>(csvFileName);

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); // , 를 기준으로 나뉨

            Dialogue dialogue = new Dialogue(); // 대사 리스트 생성

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
