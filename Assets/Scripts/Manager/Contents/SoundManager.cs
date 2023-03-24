using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletomManager<SoundManager>
{
    // AudioSource�� ������ ������Ʈ�̱� ������ �� ������Ʈ�� ���� ����� ����� ����� �� ����.
    AudioSource[] m_AudioSources = new AudioSource[(int)Defines.Sound.MaxCount];

    // AudioClip Casing ������ �ϱ� ���� Dictionary �� ����
    // �Ϲ������� ȿ���� ���� ���� ���� ���� ���̱� ������ �� �������� Load �� �ϱ⿡�� ���ϰ� �� �� ����.
    Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();

    bool isPlayingTrack01;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");

        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            // ���� �̸� ���� / Defines Sound �� ����� �����Ѵ�.
            string[] soundNames = System.Enum.GetNames(typeof(Defines.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                m_AudioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            m_AudioSources[(int)Defines.Sound.Bgm01].loop = true; // BGM �� Loop ����
            m_AudioSources[(int)Defines.Sound.Bgm02].loop = true;
            isPlayingTrack01 = true;
        }
    }

    // �޸� ����
    public void Clear()
    {
        foreach (AudioSource audioSource in m_AudioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        m_AudioClips.Clear();
    }

    public void Play(string path, Defines.Sound type = Defines.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);

        // * �ٸ� ������ �Լ��� ȣ���ϴ� ���� �� �����ؼ� ��������� �Ѵ�.
        // ���� ���� ������ ���ӵǾ� �ߺ��Ǵ� �Լ����� �þ�� �Ǹ� ���װ� �Ͼ ���ɼ��� �ſ� ũ��.
    }

    public void Play(AudioClip audioClip, Defines.Sound type = Defines.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Defines.Sound.Bgm)
        {
            StopAllCoroutines();
            StartCoroutine(FadeBgmTrack(audioClip, pitch));
            isPlayingTrack01 = !isPlayingTrack01;
        }
        else
        {
            AudioSource audioSource = m_AudioSources[(int)Defines.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Defines.Sound type = Defines.Sound.Effect)
    {
        // ���� �Ǽ��� ��� ������ �������� ��� �ڵ� ���� �ǵ��� ����
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Defines.Sound.Bgm)
        {
            audioClip = ResourcesManager.Instance.Load<AudioClip>(path);
        }
        else
        {
            // Dictionary �� �ش��ϴ� path ���� ���ٸ� ���� ���� �� ����� ����
            if (m_AudioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = ResourcesManager.Instance.Load<AudioClip>(path);
                m_AudioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.LogError($"Audio Clip Missing ! {path}");

        // Path �� ������ �����ϰ� �ִٸ� �״�� audioClip �� ������
        return audioClip;
    }

    private IEnumerator FadeBgmTrack(AudioClip audioClip, float pitch = 1.0f)
    {
        float timeToFade = 3;
        float timeElapsed = 0;

        if (isPlayingTrack01)
        {
            m_AudioSources[(int)Defines.Sound.Bgm02].pitch = pitch;
            m_AudioSources[(int)Defines.Sound.Bgm02].clip = audioClip;
            m_AudioSources[(int)Defines.Sound.Bgm02].Play();

            while (timeElapsed < timeToFade)
            {
                m_AudioSources[(int)Defines.Sound.Bgm02].volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                m_AudioSources[(int)Defines.Sound.Bgm01].volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            m_AudioSources[(int)Defines.Sound.Bgm01].Stop();
        }
        else
        {
            m_AudioSources[(int)Defines.Sound.Bgm01].pitch = pitch;
            m_AudioSources[(int)Defines.Sound.Bgm01].clip = audioClip;
            m_AudioSources[(int)Defines.Sound.Bgm01].Play();

            while (timeElapsed < timeToFade)
            {
                m_AudioSources[(int)Defines.Sound.Bgm01].volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                m_AudioSources[(int)Defines.Sound.Bgm02].volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            m_AudioSources[(int)Defines.Sound.Bgm02].Stop();
        }
    }
}
