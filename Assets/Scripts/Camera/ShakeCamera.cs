using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeCamera : SingletomManager<ShakeCamera>
{
    [SerializeField]
    private CinemachineFreeLook m_FreeLook;

    [SerializeField]
    private float m_ShakerTimer;
    [SerializeField]
    private float m_ShakeTimerTotal;
    [SerializeField]
    private float m_StartintIntensity;

    void Awake()
    {
        m_FreeLook = GetComponent<CinemachineFreeLook>();
    }

    public void CameraShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin
            = m_FreeLook.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        m_StartintIntensity = intensity;
        m_ShakeTimerTotal = time;
        m_ShakerTimer = time;
    }

    void Update()
    {
        if (m_ShakerTimer > 0)
        {
            m_ShakerTimer -= Time.deltaTime;
            if (m_ShakerTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin
                    = m_FreeLook.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
