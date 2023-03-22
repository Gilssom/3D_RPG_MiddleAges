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
    [SerializeField]
    private QuestTargetMarker m_QuestMarker;
    public NpcInfo m_NpcInfo;

    [Header("퀘스트 진행 관련 Event")]
    public bool isQuestComplete;
    public GameObject m_CompleteableMarkerPrefab;

    [Header("서브 퀘스트 관련 Event")]
    public bool isSubQuest;
    public Quest[] m_SubQuest;

    [Header("대화 시작 / 종료 이벤트")]
    public UnityEngine.Events.UnityEvent onTalkStart;
    public UnityEngine.Events.UnityEvent onTalkEnd;

    [Header("대화 관련 오브젝트")]
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

    public void UpdateQuestState()
    {
        foreach (var quest in m_SubQuest)
        {
            if (m_SubQuest == null)
                break;

            if (quest.p_IsAcceptable && !QuestSystem.Instance.ContainsInCompleteQuests(quest) && !isSubQuest)
            {
                if(m_QuestMarker.gameObject.activeSelf == false)
                    m_QuestMarker.gameObject.SetActive(true);

                isSubQuest = true;            
                SubQuestMarker(quest);
            }
        }
    }

    public void SubQuestMarker(Quest quest) =>
        GetComponentInChildren<QuestTargetMarker>().SubQuestTargetQuest(quest, quest.p_IsRegistered);

    public void InteractionNpc()
    {
        if (isSubQuest)
        {
            foreach (var quest in m_SubQuest)
            {
                if (quest.p_IsAcceptable && !QuestSystem.Instance.ContainsInCompleteQuests(quest))
                    InventoryManager.Instance.TryOpenSubQuestSystem(quest);
            }

            return;
        }

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
        var questMarker = GetComponentInChildren<QuestTargetMarker>();

        if (questMarker)
        {
            bool tryTalk = questMarker.isQuestTarget;

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

        // Npc 위에 물음표 마커를 띄운다.
        // 대화를 걸면 완료하는 대화창이 뜨고,
        // 대화가 끝나면 퀘스트를 완료 시킨다.
        if (GameScene.Instance.m_CurQuest.p_IsCompletable)
        {
            Debug.Log("완료할 수 있는 퀘스트가 존재합니다");
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

    public void SetCamera(bool isTalk)
    {
        m_TalkCamera.SetActive(isTalk);
    }
}
