using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInfo : UI_Popup
{
    enum GameObjects
    {
        InfoPanel,
    }

    enum Texts
    {
        MaxHpText,
        LevelText,
        AttackText,
        DefenseText,
        CriChanceText,
        CriDamageText,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        SetPlayerInfo();

        GetObject((int)GameObjects.InfoPanel).SetActive(false);
    }

    void SetPlayerInfo()
    {
        GetText((int)Texts.LevelText).text = $"현재 레벨 : {BaseInfo.playerInfo.Level}"; 
        GetText((int)Texts.MaxHpText).text = $"최대 생명력 : {BaseInfo.playerInfo.MaxHp}";
        GetText((int)Texts.AttackText).text = $"현재 공격력 : {BaseInfo.playerInfo.Attack}";
        GetText((int)Texts.DefenseText).text = $"방어력 : {BaseInfo.playerInfo.Defense}";
        GetText((int)Texts.CriChanceText).text = $"치명타 확률 : {BaseInfo.playerInfo.CriticalChance}%";
        GetText((int)Texts.CriDamageText).text = $"치명타 공격력 : {BaseInfo.playerInfo.CriticalDamage}%";
    }

    public void OpenPlayerInfo()
    {
        GetObject((int)GameObjects.InfoPanel).SetActive(true);
        SetPlayerInfo();
    }

    public void ClosePlayerInfo()
    {
        GetObject((int)GameObjects.InfoPanel).SetActive(false);
    }
}
