using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : UI_Popup
{
    enum GameObjects
    {
        UI_DialogueBar,
        UI_Back,
    }

    enum Texts
    {
        UI_DialogueText,
        UI_DialogueName,
    }

    TalkData[] m_Dialogues;
    NpcInteraction m_CurNpc;

    bool isDialogue = false;
    bool isNext = false;

    public bool DoDialogue { get { return isDialogue; } set { isDialogue = value; } }

    [Header("텍스트 출력 딜레이")]
    [SerializeField]
    float m_TextDelay;

    int m_LineCount = 0;
    int m_ContextCount = 0;

    WaitForSeconds m_DelaySeconds;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        m_DelaySeconds = new WaitForSeconds(m_TextDelay);

        GetObject((int)GameObjects.UI_DialogueBar).SetActive(false);
        GetObject((int)GameObjects.UI_Back).SetActive(false);
    }

    public void DialogueNext()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                isNext = false;
                GetText((int)Texts.UI_DialogueText).text = "";

                if (++m_ContextCount < m_Dialogues[m_LineCount].m_Contexts.Length)
                {
                    StartCoroutine(TypeWriter());
                }
                else
                {
                    m_ContextCount = 0;

                    if (++m_LineCount < m_Dialogues.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        EndDialogue();
                    }
                }
            }
        }
    }

    public void ShowDialogue(TalkData[] dialogues, NpcInteraction npc)
    {
        m_CurNpc = npc;
        isDialogue = true;
        GetObject((int)GameObjects.UI_DialogueBar).SetActive(true);
        GetObject((int)GameObjects.UI_Back).SetActive(true);
        GetText((int)Texts.UI_DialogueText).text = "";
        GetText((int)Texts.UI_DialogueName).text = "";
        m_Dialogues = dialogues;

        StartCoroutine(TypeWriter());
    }

    void EndDialogue()
    {
        m_CurNpc.EndTalk();
        isDialogue = false;
        m_ContextCount = 0;
        m_LineCount = 0;
        m_Dialogues = null;
        isNext = false;
        m_CurNpc = null;
        SettingUI(false);
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);

        string replaceText = m_Dialogues[m_LineCount].m_Contexts[m_ContextCount];
        replaceText = replaceText.Replace("'", ",");
        replaceText = replaceText.Replace("\\n", "\n"); // 줄바꿈

        bool white = false, yellow = false;
        bool ignore = false;

        for (int i = 0; i < replaceText.Length; i++)
        {
            // 색 변경
            switch (replaceText[i])
            {
                case 'ⓦ':white = true; yellow = false; ignore = true; break;
                case 'ⓨ': white = false; yellow = true; ignore = true; break;
            }

            string letter = replaceText[i].ToString();

            if (!ignore)
            {
                if (white) { letter = "<color=#FFFFFF>" + letter + "</color>"; }
                else if (yellow) { letter = "<color=#F8F052>" + letter + "</color>"; }

                GetText((int)Texts.UI_DialogueText).text += letter;
            }

            ignore = false;

            yield return m_DelaySeconds;
        }

        isNext = true;
    }

    void SettingUI(bool flag)
    {
        GetObject((int)GameObjects.UI_DialogueBar).SetActive(flag);
        GetObject((int)GameObjects.UI_Back).SetActive(flag);

        if (flag)
        {
            if (m_Dialogues[m_LineCount].m_Name == "")
            {
                GetObject((int)GameObjects.UI_DialogueBar).SetActive(false);
                GetObject((int)GameObjects.UI_Back).SetActive(false);
            }
            else
            {
                GetObject((int)GameObjects.UI_DialogueBar).SetActive(true);
                GetObject((int)GameObjects.UI_Back).SetActive(true);
                GetText((int)Texts.UI_DialogueName).text = m_Dialogues[m_LineCount].m_Name;
            }
        }
    }
}
