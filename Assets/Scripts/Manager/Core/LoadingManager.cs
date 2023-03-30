using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // 비동기 방식 > 다른작업 가능
        op.allowSceneActivation = false; // 씬을 넘길지 안넘길지

        float timer = -5f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.3f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.3f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    //FadeInOutManager.Instance.OutStartFadeAnim(nextScene);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
