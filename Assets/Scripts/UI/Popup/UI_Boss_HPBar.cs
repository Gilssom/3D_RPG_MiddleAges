using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss_HPBar : UI_Popup
{
    enum GameObjects
    {
        UI_Boss_Slider,
    }

    enum Images
    {
        UI_Boss_Image,
    }

    enum Texts
    {
        UI_Boss_Name,
        UI_Boss_Info,
    }

    public BossInfo m_BossInfo;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        SetInfo(m_BossInfo.Name, m_BossInfo.Level);
    }

    void Update()
    {
        float ratio = m_BossInfo.Hp / (float)m_BossInfo.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        if (ratio <= 0)
        {
            UIManager.Instance.isBossHPOpen = false;
            ResourcesManager.Instance.Destroy(this.gameObject);
        }

        GetObject((int)GameObjects.UI_Boss_Slider).GetOrAddComponet<Slider>().value = ratio;
    }

    public void SetInfo(string name, int level)
    {
        GetText((int)Texts.UI_Boss_Name).text = name;
        GetText((int)Texts.UI_Boss_Info).text = $"Lv. {level}  |  네임드  |  악마형";
    }
}
