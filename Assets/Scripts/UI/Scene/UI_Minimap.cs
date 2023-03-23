using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Minimap : UI_Scene
{
    enum Texts
    {
        MiniMap_Name
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));

        MinimapNameSetText();
    }

    public void MinimapNameSetText()
    {
        GetText((int)Texts.MiniMap_Name).text = BaseInfo.playerInfo.m_Player.m_CurrentAreaName;
    }
}
