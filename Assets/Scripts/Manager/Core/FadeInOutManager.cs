using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutManager : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    //public float m_FadeTime = 2f;
    float m_AccumTime = 0f;

    private Coroutine m_FadeCor;

    private void Awake()
    {
        m_CanvasGroup = gameObject.GetComponentInChildren<CanvasGroup>();
    }

    //private void OnEnable()
    //{
    //    StartFadeIn();
    //}

    public void StartFadeIn(float customFadeTime)
    {
        if (m_FadeCor != null)
        {
            StopAllCoroutines();
            m_FadeCor = null;
        }
        m_FadeCor = StartCoroutine(FadeIn(customFadeTime));

    }

    public void StartFadeOut(float customFadeTime)
    {
        if (m_FadeCor != null)
        {
            StopAllCoroutines();
            m_FadeCor = null;
        }
        m_FadeCor = StartCoroutine(FadeOut(customFadeTime));

    }

    private IEnumerator FadeIn(float customFadeTime)
    {
        m_AccumTime = 0f;
        while (m_AccumTime < customFadeTime)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(0f, 1f, m_AccumTime / customFadeTime);
            yield return 0;
            m_AccumTime += Time.deltaTime;
        }
        m_CanvasGroup.alpha = 1f;
        StartFadeOut(customFadeTime);
    }


    private IEnumerator FadeOut(float customFadeTime)
    {
        m_AccumTime = 0f;
        while (m_AccumTime < customFadeTime)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(1f, 0f, m_AccumTime / customFadeTime);
            yield return 0;
            m_AccumTime += Time.deltaTime;
        }
        m_CanvasGroup.alpha = 0f;
    }
}
