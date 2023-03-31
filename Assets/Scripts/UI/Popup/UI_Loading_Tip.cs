using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading_Tip : UI_Popup
{
    [SerializeField]
    private string[] m_TipText;

    enum Texts
    {
        TipText,
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));

        TipText();
    }

    void TipText()
    {
        int randomTip = Random.Range(0, 5);

        GetText((int)Texts.TipText).text = m_TipText[randomTip];
    }
}
