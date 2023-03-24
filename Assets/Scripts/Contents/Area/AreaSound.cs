using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [Header("���� ���� �������")]
    public AudioClip m_PrevTrack;
    [Header("�ش� ���� �������")]
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
