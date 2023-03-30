using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerEx : SingletomManager<SceneManagerEx>
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public FadeInOutManager m_BlackScreenUI { get; private set; }

    private void Awake()
    {
        m_BlackScreenUI = ResourcesManager.Instance.Instantiate($"UI/Scene/UI_ScreenBlack", transform).GetComponent<FadeInOutManager>();
    }

    public void LoadScene(Defines.Scene type)
    {
        CurrentScene.Clear();
        //SceneManager.LoadScene(GetSceneName(type));
        SceneManager.LoadScene("LoadingScene");
        LoadingScene.m_NextScene = GetSceneName(type);
    }

    // Defines 의 Scene Name 을 string 으로 가져오기
    string GetSceneName(Defines.Scene type)
    {
        string name = System.Enum.GetName(typeof(Defines.Scene), type);
        return name;
    }
}
