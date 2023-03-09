using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcInfo
{
    public Defines.NpcType m_NpcType;
    public bool isNearPlayer;
    public bool AnimTrigger;
    public int curTalkIndex;
    public string[] TalkEventData;
}

public class NpcInteraction : MonoBehaviour
{
    private Animator m_Anim;
    private Dialogue m_TalkData;
    public NpcInfo m_NpcInfo;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_TalkData = GetComponent<Dialogue>();

        if (m_NpcInfo.TalkEventData.Length > m_NpcInfo.curTalkIndex)
            m_TalkData.m_EventName = m_NpcInfo.TalkEventData[m_NpcInfo.curTalkIndex];
    }

    void Update()
    {
        SetOutLine();
    }

    public void InteractionNpc()
    {
        switch (m_NpcInfo.m_NpcType)
        {
            case Defines.NpcType.Enforce:
                InventoryManager.Instance.TryOpenEnforceShop();
                break;
            case Defines.NpcType.Smith:
                InventoryManager.Instance.TryOpenEnforceSystem();
                break;
            case Defines.NpcType.Shop:
                InventoryManager.Instance.TryOpenPotionShop();
                break;
            case Defines.NpcType.Alchemy:
                break;
            case Defines.NpcType.Normal:
                SetTalkData();
                break;
        }
    }

    void SetTalkData()
    {
        TalkData[] talkDatas = GetComponent<Dialogue>().GetObejctDialogue();

        InventoryManager.Instance.m_Dialogue.ShowDialogue(talkDatas, this);

        if (talkDatas != null)
            DebugDialogue(talkDatas);
    }

    public void EndTalk()
    {
        m_NpcInfo.curTalkIndex++;

        if (m_NpcInfo.TalkEventData.Length > m_NpcInfo.curTalkIndex)
            m_TalkData.m_EventName = m_NpcInfo.TalkEventData[m_NpcInfo.curTalkIndex];
        else
            return;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_NpcInfo.isNearPlayer = true;

            if(m_NpcInfo.AnimTrigger)
                SetNpcAnim();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_NpcInfo.isNearPlayer = false;

            if (m_NpcInfo.AnimTrigger)
                SetNpcAnim();
        }
    }

    void SetNpcAnim()
    {
        if (m_Anim != null)
        {
            m_Anim.SetBool("NearPlayer", m_NpcInfo.isNearPlayer);
        }
    }

    void SetOutLine()
    {
        if (BaseInfo.playerInfo.m_Player.m_NearNpc == this)
        {
            // 플레이어와 가장 가까운 Npc가 자신이라면
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }
    }
}
