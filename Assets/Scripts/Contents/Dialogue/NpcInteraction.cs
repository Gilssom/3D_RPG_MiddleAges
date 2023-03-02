using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    public void SetTalkData()
    {
        TalkData[] talkDatas = GetComponent<Dialogue>().GetObejctDialogue();

        if (talkDatas != null)
            DebugDialogue(talkDatas);
    }

    void DebugDialogue(TalkData[] talkDatas)
    {
        for (int i = 0; i < talkDatas.Length; i++)
        {
            // 캐릭터 이름 출력
            Debug.Log(talkDatas[i].m_Name);
            // 대사들 출력
            foreach (string context in talkDatas[i].m_Contexts)
                Debug.Log(context);
        }
    }
}
