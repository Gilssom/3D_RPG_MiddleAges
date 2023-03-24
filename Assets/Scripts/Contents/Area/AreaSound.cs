using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [Header("이전 지역 배경음악")]
    public AudioClip m_PrevTrack;
    [Header("해당 지역 배경음악")]
    public AudioClip m_NewTrack;

    int i;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            i++;

            if (i % 2 == 0)
                SoundManager.Instance.Play(m_PrevTrack, Defines.Sound.Bgm);
            else
                SoundManager.Instance.Play(m_NewTrack, Defines.Sound.Bgm);
        }
    }
}
