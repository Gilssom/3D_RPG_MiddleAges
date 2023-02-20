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
        GetText((int)Texts.LevelText).text = $"���� ���� : {BaseInfo.playerInfo.Level}"; 
        GetText((int)Texts.MaxHpText).text = $"�ִ� ����� : {BaseInfo.playerInfo.MaxHp}";
        GetText((int)Texts.AttackText).text = $"���� ���ݷ� : {BaseInfo.playerInfo.Attack}";
        GetText((int)Texts.DefenseText).text = $"���� : {BaseInfo.playerInfo.Defense}";
        GetText((int)Texts.CriChanceText).text = $"ġ��Ÿ Ȯ�� : {BaseInfo.playerInfo.CriticalChance}%";
        GetText((int)Texts.CriDamageText).text = $"ġ��Ÿ ���ݷ� : {BaseInfo.playerInfo.CriticalDamage}%";
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
