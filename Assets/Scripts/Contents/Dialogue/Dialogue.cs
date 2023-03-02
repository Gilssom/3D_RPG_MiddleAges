using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkData
{
    [Tooltip("현재 대사 캐릭터 이름")]
    public string m_Name;

    [Tooltip("현재 대사 내용")]
    public string[] m_Contexts;
}

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    // 대화 이벤트 이름
    [SerializeField] string m_EventName;

    // 위에서 선언한 TalkData 배열 
    [SerializeField] TalkData[] m_TalkDatas;

    public TalkData[] GetObejctDialogue()
    {
        return DialogueParser.GetDialogue(m_EventName);
    }
}

[System.Serializable]
public class DebugTalkData
{
    public string m_EventName;
    public TalkData[] m_TalkDatas;

    public DebugTalkData(string name, TalkData[] td)
    {
        m_EventName = name;
        m_TalkDatas = td;
    }
}