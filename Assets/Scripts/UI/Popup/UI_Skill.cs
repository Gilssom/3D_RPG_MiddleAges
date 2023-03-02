using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill : UI_Popup
{
    enum GameObjects
    {
        UI_SkillWindow,
        UI_SkillContent,
        UI_SkillContent_Pv,
    }

    enum Buttons
    {
        UI_LeftButton,
        UI_RightButton,
        UI_LeftButton_Pv,
        UI_RightButton_Pv,
        UI_AcceptButton
    }

    enum Texts
    {
        UI_CategoryName,
        UI_CategoryName_Pv,
    }

    private GameObject[] m_KeyUIs;

    private NMActiveSkill m_CurrentSkill;

    [SerializeField]
    private int m_ChangePos;

    public int ChangePos { get { return m_ChangePos; } set { m_ChangePos = value; } }

    public override void Init()
    {
        base.Init();

        m_KeyUIs = new GameObject[4];

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.UI_LeftButton).gameObject.BindEvent(CategoryLeftButtonPress);
        GetButton((int)Buttons.UI_RightButton).gameObject.BindEvent(CategoryRightButtonPress);

        GetButton((int)Buttons.UI_LeftButton_Pv).gameObject.BindEvent(CategoryLeftButtonPress_Pv);
        GetButton((int)Buttons.UI_RightButton_Pv).gameObject.BindEvent(CategoryRightButtonPress_Pv);

        GetButton((int)Buttons.UI_AcceptButton).gameObject.BindEvent(SkillChangeAccept);

        GetText((int)Texts.UI_CategoryName).text = NightMareSkillCg.ToString();

        GenerateSkillIcons(NightMareSkillCg);
        GenerateSkillIcons_Pv(NightMareSkillCg_Pv);

        for (int i = 0; i < InventoryManager.Instance.m_SkillQuickSlot.m_ButtonList.Length; i++)
        {
            m_KeyUIs[i] = InventoryManager.Instance.m_SkillQuickSlot.m_ButtonList[i];
        }

        GetObject((int)GameObjects.UI_SkillWindow).SetActive(false);
    }

    public void OpenEnforce()
    {
        GetObject((int)GameObjects.UI_SkillWindow).SetActive(true);
    }

    public void CloseEnforce()
    {
        GetObject((int)GameObjects.UI_SkillWindow).SetActive(false);
    }

    // -----------  아래부터 각 카테고리 별 실행 코드 ------------

    #region #Active Skill Category
    private NMSkillCategory NightMareSkillCg;
    private int m_CategoryPos;

    void CategoryLeftButtonPress(PointerEventData data)
    {
        m_CategoryPos--;
        SetCategory();
    }

    void CategoryRightButtonPress(PointerEventData data)
    {
        m_CategoryPos++;
        SetCategory();
    }

    void SetCategory()
    {
        if (m_CategoryPos == -1)
            m_CategoryPos = 2;

        if (m_CategoryPos == 3)
            m_CategoryPos = 0;

        NightMareSkillCg = (NMSkillCategory)m_CategoryPos;
        GetText((int)Texts.UI_CategoryName).text = NightMareSkillCg.ToString();
        GenerateSkillIcons(NightMareSkillCg);
    }

    void GenerateSkillIcons(NMSkillCategory category)
    {
        // 거꾸로 진행하는 이유 :: 0부터 지우면 childCount가 즉시 수정되기 때문
        for (int i = GetObject((int)GameObjects.UI_SkillContent).transform.childCount - 1; i >= 0; i--)
        {
            ResourcesManager.Instance.Destroy(GetObject((int)GameObjects.UI_SkillContent).transform.GetChild(i).gameObject);
        }

        foreach (NMActiveSkill skill in SkillManager.Instance.NMActiveSkills)
        {
            if (skill.m_Category == category)
            {
                GameObject go = ResourcesManager.Instance.Instantiate("UI/Skill/UI_Skill_Slot", GetObject((int)GameObjects.UI_SkillContent).transform);
                go.GetComponent<Image>().sprite = skill.m_SkillIcon;
                go.GetComponentInChildren<Text>().text = skill.m_Name;
                go.GetComponent<Button>().onClick.AddListener(() => SkillButtonPress(go.GetComponentInChildren<Button>()));
                go.GetOrAddComponet<UI_Skill_Slot>().m_Item = ResourcesManager.Instance.Load<Item>($"Data/ItemData/SkillDesc/{skill.m_Description}");
            }
        }
    }

    void SkillChangeAccept(PointerEventData data)
    {
        if (m_ChangePos == 0 || m_CurrentSkill == null)
            return;

        ChangeSkillKey(m_ChangePos, m_CurrentSkill);
    }

    void ChangeSkillKey(int where, NMActiveSkill skill)
    {
        m_KeyUIs[where - 1].GetComponent<Image>().sprite = skill.m_SkillIcon;
        SkillKeyMap.Instance.SetKeyFunc(where, skill.SkillExecute);
    }

    void SkillButtonPress(Button btn)
    {
        foreach (NMActiveSkill skill in SkillManager.Instance.NMActiveSkills)
        {
            if (skill.m_Name == btn.GetComponentInChildren<Text>().text)
            {
                m_CurrentSkill = skill;
            }
        }
    }
    #endregion

    #region #Passive Skill Category
    private NMSkillCategory NightMareSkillCg_Pv;
    private int m_CategoryPos_Pv;

    void CategoryLeftButtonPress_Pv(PointerEventData data)
    {
        m_CategoryPos_Pv--;
        SetCategory_Pv();
    }

    void CategoryRightButtonPress_Pv(PointerEventData data)
    {
        m_CategoryPos_Pv++;
        SetCategory_Pv();
    }

    void SetCategory_Pv()
    {
        if (m_CategoryPos_Pv == -1)
            m_CategoryPos_Pv = 2;

        if (m_CategoryPos_Pv == 3)
            m_CategoryPos_Pv = 0;

        NightMareSkillCg_Pv = (NMSkillCategory)m_CategoryPos_Pv;
        GetText((int)Texts.UI_CategoryName_Pv).text = NightMareSkillCg_Pv.ToString();
        GenerateSkillIcons_Pv(NightMareSkillCg_Pv);
    }

    void GenerateSkillIcons_Pv(NMSkillCategory category)
    {
        // 거꾸로 진행하는 이유 :: 0부터 지우면 childCount가 즉시 수정되기 때문
        for (int i = GetObject((int)GameObjects.UI_SkillContent_Pv).transform.childCount - 1; i >= 0; i--)
        {
            ResourcesManager.Instance.Destroy(GetObject((int)GameObjects.UI_SkillContent_Pv).transform.GetChild(i).gameObject);
        }

        foreach (NMPassiveSkill skill in SkillManager.Instance.NMPassiveSkills)
        {
            if (skill.m_Category == category)
            {
                GameObject go = ResourcesManager.Instance.Instantiate("UI/Skill/UI_Skill_Slot", GetObject((int)GameObjects.UI_SkillContent_Pv).transform);
                go.GetComponent<Image>().sprite = skill.m_SkillIcon;
                go.GetComponentInChildren<Text>().text = skill.m_Name;
                go.GetComponent<Button>().onClick.AddListener(() => ResetCurSkill());
                go.GetOrAddComponet<UI_Skill_Slot>().m_Item = ResourcesManager.Instance.Load<Item>($"Data/ItemData/SkillDesc/{skill.m_Description}");
            }
        }
    }

    void ResetCurSkill()
    {
        m_CurrentSkill = null;
    }
    #endregion
}
