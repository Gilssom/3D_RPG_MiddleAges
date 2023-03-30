using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    public static string m_NextScene;

    [SerializeField]
    Slider progressBar;

    protected override void Init()
    {
        base.Init();

        SceneType = Defines.Scene.Loading;
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess(m_NextScene));
    }

    IEnumerator LoadSceneProcess(string sceneName)
    {
        //AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); // 비동기 방식 > 다른작업 가능
        //op.allowSceneActivation = false; // 씬을 넘길지 안넘길지

        //float timer = -5f;
        //while (!op.isDone)
        //{
        //    yield return null;

        //    if (op.progress < 0.3f)
        //    {
        //        progressBar.value = op.progress;
        //    }
        //    else
        //    {
        //        timer += Time.unscaledDeltaTime;
        //        progressBar.value = Mathf.Lerp(0.3f, 1f, timer);
        //        if (progressBar.value >= 1f)
        //        {
        //            //FadeInOutManager.Instance.OutStartFadeAnim(nextScene);
        //            SceneManager.LoadScene(sceneName);
        //            op.allowSceneActivation = true;
        //            yield break;
        //        }
        //    }
        //}

        float timer = -5f;
        while (progressBar.value < 1f)
        {
            yield return null;

            timer += Time.unscaledDeltaTime;
            progressBar.value = Mathf.Lerp(0f, 1f, timer);

            if (progressBar.value >= 1f)
            {
                SceneManager.LoadScene(sceneName);
                yield break;
            }
        }
    }

    public override void Clear()
    {
        Debug.Log("Loading Scene Clear");
        GameManager.Clear();
    }
}