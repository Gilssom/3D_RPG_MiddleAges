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

    [Header("����Ʈ ���� ���� Event")]
    //public bool isQuestNpc;
    //public Quest[] m_Quest;
    //public int m_QuestIndex;
    public bool isQuestComplete;
    public GameObject m_CompleteableMarkerPrefab;

    [Header("��ȭ ���� / ���� �̺�Ʈ")]
    public UnityEngine.Events.UnityEvent onTalkStart;
    public UnityEngine.Events.UnityEvent onTalkEnd;

    [Header("��ȭ ���� ������Ʈ")]
    public GameObject m_TalkCamera;

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
        // isQuestComplete �� Ȱ��ȭ �Ǹ� ��ȭ�� �ѹ� �� �ɾ �Ϸ� ��Ų��.
        var targetMarker = GetComponentInChildren<QuestTargetMarker>();

        if (targetMarker)
        {
            bool tryTalk = targetMarker.isQuestTarget;

            if (tryTalk)
                TalkEvent();
        }
        else
        {
            if(isQuestComplete)
                TalkEvent();
        }
    }

    void TalkEvent()
    {
        onTalkStart.Invoke();

        TalkData[] talkDatas = GetComponent<Dialogue>().GetObejctDialogue();

        InventoryManager.Instance.m_Dialogue.ShowDialogue(talkDatas, this);

        if (talkDatas != null)
            DebugDialogue(talkDatas);
    }

    public void EndTalk()
    {
        m_NpcInfo.curTalkIndex++;
        onTalkEnd.Invoke();

        if (isQuestComplete)
        {
            GameScene.Instance.m_CurQuest.Complete();
            isQuestComplete = false;
            m_CompleteableMarkerPrefab.SetActive(false);
        }

        // Npc ���� ����ǥ ��Ŀ�� ����.
        // ��ȭ�� �ɸ� �Ϸ��ϴ� ��ȭâ�� �߰�,
        // ��ȭ�� ������ ����Ʈ�� �Ϸ� ��Ų��.
        if (GameScene.Instance.m_CurQuest.p_IsCompletable)
        {
            Debug.Log("�Ϸ��� �� �ִ� ����Ʈ�� �����մϴ�");
            m_CompleteableMarkerPrefab.SetActive(true);
            isQuestComplete = true;
        }

        if (m_NpcInfo.TalkEventData.Length > m_NpcInfo.curTalkIndex)
            m_TalkData.m_EventName = m_NpcInfo.TalkEventData[m_NpcInfo.curTalkIndex];
        else
            return;
    }

    void DebugDialogue(TalkData[] talkDatas)
    {
        for (int i = 0; i < talkDatas.Length; i++)
        {
            // ĳ���� �̸� ���
            Debug.Log(talkDatas[i].m_Name);
            // ���� ���
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
            // �÷��̾�� ���� ����� Npc�� �ڽ��̶��
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    public void SetCamera(bool isTalk)
    {
        m_TalkCamera.SetActive(isTalk);
    }
}
