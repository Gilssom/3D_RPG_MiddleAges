using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkData
{
    [Tooltip("���� ��� ĳ���� �̸�")]
    public string m_Name;

    [Tooltip("���� ��� ����")]
    public string[] m_Contexts;
}

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    // ��ȭ �̺�Ʈ �̸�
    [SerializeField] string m_EventName;

    // ������ ������ TalkData �迭 
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