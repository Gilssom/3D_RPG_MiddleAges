using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletomManager<SoundManager>
{
    // AudioSource는 일종의 컴포넌트이기 때문에 빈 오브젝트를 만들어서 등록을 해줘야 사용할 수 있음.
    AudioSource[] m_AudioSources = new AudioSource[(int)Defines.Sound.MaxCount];

    // AudioClip Casing 역할을 하기 위해 Dictionary 를 구축
    // 일반적으로 효과음 같은 경우는 자주 사용될 것이기 때문에 매 순간마다 Load 를 하기에는 부하가 올 수 있음.
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

            // 사운드 이름 추출 / Defines Sound 의 목록을 추출한다.
            string[] soundNames = System.Enum.GetNames(typeof(Defines.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                m_AudioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            m_AudioSources[(int)Defines.Sound.Bgm01].loop = true; // BGM 은 Loop 상태
            m_AudioSources[(int)Defines.Sound.Bgm02].loop = true;
            isPlayingTrack01 = true;
        }
    }

    // 메모리 정리
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

        // * 다른 버전의 함수를 호출하는 것을 꼭 연습해서 사용했으면 한다.
        // 복붙 복붙 복붙이 연속되어 중복되는 함수들이 늘어나게 되면 버그가 일어날 가능성이 매우 크다.
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
        // 만약 실수로 경로 지정을 안해줬을 경우 자동 지정 되도록 설정
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Defines.Sound.Bgm)
        {
            audioClip = ResourcesManager.Instance.Load<AudioClip>(path);
        }
        else
        {
            // Dictionary 에 해당하는 path 값이 없다면 새로 생성 및 등록을 해줌
            if (m_AudioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = ResourcesManager.Instance.Load<AudioClip>(path);
                m_AudioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.LogError($"Audio Clip Missing ! {path}");

        // Path 를 추적해 존재하고 있다면 그대로 audioClip 을 꺼내옴
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
