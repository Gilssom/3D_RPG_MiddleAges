using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : UI_Base
{
    enum Images
    {
        ButtonCoolTime,
    }

    private float m_CurCoolTime;
    private bool m_isCoolTime;

    public bool isCoolTime { get { return m_isCoolTime; } set { m_isCoolTime = value; } }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    public void ResetCoolTime(float coolTime, int keyNumber)
    {
        m_CurCoolTime = coolTime;
        m_isCoolTime = true;
        StartCoroutine(TestCoolTime(coolTime));
    }

    IEnumerator TestCoolTime(float coolTime)
    {
        while (m_isCoolTime)
        {
            m_CurCoolTime -= Time.deltaTime;

            GetImage((int)Images.ButtonCoolTime).fillAmount = m_CurCoolTime / coolTime;

            if (m_CurCoolTime <= 0)
            {
                m_isCoolTime = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
