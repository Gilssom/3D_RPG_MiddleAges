using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_GUI : UI_Scene
{
    public enum GameObjects
    {
        HPBar,
        ExpBar,
    }

    enum Texts
    {
        TimeCount,
        GoldCount,
        FragCount,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        if (IsInvoking("UpdateTime"))
            CancelInvoke("UpdateTime");

        InvokeRepeating("UpdateTime", 0, 0.2f);
    }

    void Update()
    {
        // 각각의 오브젝트에서 가지고 있는 Stat 에 의해 HP Bar 변경
        float ratio = BaseInfo.playerInfo.Hp / (float)BaseInfo.playerInfo.MaxHp;
        float expRatio = BaseInfo.playerInfo.Exp / (float)BaseInfo.playerInfo.MaxExp;
        SetRatio(GameObjects.HPBar, ratio);
        SetRatio(GameObjects.ExpBar, expRatio);
        SetText();
    }

    public void SetRatio(GameObjects objects, float ratio)
    {
        if (ratio < 0)
            ResourcesManager.Instance.Destroy(this.gameObject);

        GetObject((int)objects).GetComponent<Slider>().value = ratio;
    }

    void UpdateTime()
    {
        string time = DateTime.Now.ToString("HH : mm");
        GetText((int)Texts.TimeCount).text = time;
    }

    void SetText()
    {
        GetText((int)Texts.GoldCount).text = string.Format("{0:#,0}", BaseInfo.playerInfo.Gold);
        GetText((int)Texts.FragCount).text = string.Format("{0:#,0}", BaseInfo.playerInfo.Fragments);
    }
}
