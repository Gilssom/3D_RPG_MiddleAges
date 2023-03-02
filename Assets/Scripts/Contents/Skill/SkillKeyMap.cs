using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void KeyFunc();

public class SkillKeyMap : UI_Scene
{
    public static SkillKeyMap Instance;

    void Awake()
    {
        Instance = this;
    }

    public GameObject[] m_ButtonList;

    public KeyFunc Key1Func;
    public KeyFunc Key2Func;
    public KeyFunc Key3Func;
    public KeyFunc Key4Func;

    public int m_CurPressButton;
    public SkillCoolTime[] m_SkillCoolTime;

    public void SetKeyFunc(int where, KeyFunc func)
    {
        switch (where)
        {
            case 1:
                Key1Func = func;
                break;
            case 2:
                Key2Func = func;
                break;
            case 3:
                Key3Func = func;
                break;
            case 4:
                Key4Func = func;
                break;
        }
    }

    public void KeyPress(int QuickNum)
    {
        m_CurPressButton = QuickNum - 1;

        switch (QuickNum)
        {
            case 1:
                if(Key1Func != null && !m_SkillCoolTime[m_CurPressButton].isCoolTime)
                    Key1Func();
                break;
            case 2:
                if (Key2Func != null && !m_SkillCoolTime[m_CurPressButton].isCoolTime)
                    Key2Func();
                break;
            case 3:
                if (Key3Func != null && !m_SkillCoolTime[m_CurPressButton].isCoolTime)
                    Key3Func();
                break;
            case 4:
                if (Key4Func != null && !m_SkillCoolTime[m_CurPressButton].isCoolTime)
                    Key4Func();
                break;
        }
    }

    public void KeyNumber(int quickNumber)
    {
        InventoryManager.Instance.m_SkillSystem.ChangePos = quickNumber;
    }

    public void ResetCoolTime(float coolTime)
    {
        m_SkillCoolTime[m_CurPressButton].ResetCoolTime(coolTime, m_CurPressButton);
    }
}
