using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    public bool IsTransparent { get; private set; } = false;

    [SerializeField]
    private MeshRenderer[] m_Renderers;
    private WaitForSeconds Delay = new WaitForSeconds(0.001f);
    private WaitForSeconds ResetDelay = new WaitForSeconds(0.002f);
    private const float alpha = 0.4f;
    private const float maxTimer = 0.2f;

    private bool isReseting = false;
    private float m_Timer = 0f;

    private Coroutine m_TimeCheckCoroutine;
    private Coroutine m_ResetCoroutine;

    void Awake()
    {
        m_Renderers = GetComponentsInChildren<MeshRenderer>();    
    }

    public void BecomeTransparent()
    {
        if(IsTransparent)
        {
            m_Timer = 0f;
            return;
        }

        // 리셋 중 다시 캐릭터를 가릴 경우
        if(m_ResetCoroutine != null && isReseting)
        {
            isReseting = false;
            IsTransparent = false;
            StopCoroutine(m_ResetCoroutine);
        }

        SetMaterialTransparent();
        IsTransparent = true;
        StartCoroutine(BecomeTransparentCoroutine());
    }

    #region #Run - time 중에 Rendering Mode 를 바꾸는 함수
    private void SetMaterialTransparent()
    {
        for (int i = 0; i < m_Renderers.Length; i++)
        {
            foreach (Material material in m_Renderers[i].materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.SetInt("_Surface", 1);

                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", false);

                material.SetOverrideTag("EnviromentObject", "Transparent");

                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }
        }
    }

    private void SetMaterialOpaque()
    {
        for (int i = 0; i < m_Renderers.Length; i++)
        {
            foreach (Material material in m_Renderers[i].materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.SetInt("_Surface", 0);

                material.renderQueue = -1;

                material.SetShaderPassEnabled("DepthOnly", true);
                material.SetShaderPassEnabled("SHADOWCASTER", true);

                material.SetOverrideTag("EnviromentObject", "Transparent");

                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
        }
    }
    #endregion

    public void ResetOriginalTransparent()
    {
        SetMaterialOpaque();
        m_ResetCoroutine = StartCoroutine(ResetOriginalTransparentCoroutine());
    }

    private IEnumerator BecomeTransparentCoroutine()
    {
        while (true)
        {
            bool isComplete = true;

            for (int i = 0; i < m_Renderers.Length; i++)
            {
                if (m_Renderers[i].material.color.a > alpha)
                    isComplete = false;

                Color color = m_Renderers[i].material.color;
                color.a -= Time.deltaTime;
                m_Renderers[i].material.color = color;
            }

            if(isComplete)
            {
                CheckTimer();
                break;
            }

            yield return Delay;
        }
    }

    private IEnumerator ResetOriginalTransparentCoroutine()
    {
        IsTransparent = false;

        while (true)
        {
            bool isComplete = true;

            for (int i = 0; i < m_Renderers.Length; i++)
            {
                if (m_Renderers[i].material.color.a < 1f)
                    isComplete = false;

                Color color = m_Renderers[i].material.color;
                color.a += Time.deltaTime;
                m_Renderers[i].material.color = color;
            }

            if(isComplete)
            {
                isReseting = false;
                break;
            }

            yield return ResetDelay;
        }
    }

    public void CheckTimer()
    {
        if (m_TimeCheckCoroutine != null)
            StopCoroutine(m_TimeCheckCoroutine);

        m_TimeCheckCoroutine = StartCoroutine(CheckTimerCoroutine());
    }

    private IEnumerator CheckTimerCoroutine()
    {
        m_Timer = 0f;

        while (true)
        {
            m_Timer += Time.deltaTime;

            if(m_Timer > maxTimer)
            {
                isReseting = true;
                ResetOriginalTransparent();
                break;
            }

            yield return null;
        }
    }
}
