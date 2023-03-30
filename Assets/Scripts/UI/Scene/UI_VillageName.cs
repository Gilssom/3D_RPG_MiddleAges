using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VillageName : UI_Scene
{
    enum Texts
    {
        VillageText
    }

    private FadeInOutManager m_FadeInOut;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));

        m_FadeInOut = gameObject.GetOrAddComponet<FadeInOutManager>();

        VillageNameSetText();
    }

    public void VillageNameSetText()
    {
        // ¸¶À» UI FadeTime = 2.5f Seconds
        m_FadeInOut.StartFadeIn(3f, true);
        GetText((int)Texts.VillageText).text = BaseInfo.playerInfo.m_Player.m_CurrentAreaName;
    }
}
