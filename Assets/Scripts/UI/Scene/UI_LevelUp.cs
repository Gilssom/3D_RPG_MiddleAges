using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUp : UI_Scene
{
    enum Texts
    {
        LevelUpText,
        LeveIUpDesc
    }

    private FadeInOutManager m_FadeInOut;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));

        m_FadeInOut = gameObject.GetOrAddComponet<FadeInOutManager>();
    }

    public void LevelUpSetText()
    {
        // 레벨업 UI FadeTime = 2.5f Seconds
        m_FadeInOut.StartFadeIn(2.5f, true);
        GetText((int)Texts.LevelUpText).text = "LEVEL UP";
        GetText((int)Texts.LeveIUpDesc).text = $"<color=#F8F052>{BaseInfo.playerInfo.Level}</color> 레벨로 올랐습니다.";
    }
}
