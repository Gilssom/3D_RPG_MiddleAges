using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Defines.Scene.Lobby;

        SoundManager.Instance.Play("Bgm/Start Scene", Defines.Sound.Bgm);
    }

    public void SceneChangeTest()
    {
        SceneManagerEx.Instance.m_BlackScreenUI.StartFadeIn(3f, false, Defines.Scene.Game01);
    }

    public void LobbySetting()
    {
        UIManager.Instance.ShowPopupUI<UI_Setting>();
    }

    public void LobbyHelper()
    {
        UIManager.Instance.ShowPopupUI<UI_Helper>();
    }

    public void CloseLobbyUI()
    {
        UIManager.Instance.ClosePopupUI();
    }

    public override void Clear()
    {
        Debug.Log("Lobby Scene Clear");
        GameManager.Clear();
    }
}
