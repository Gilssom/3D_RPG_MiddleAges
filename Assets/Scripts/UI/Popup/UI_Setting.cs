using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class UI_Setting : UI_Popup
{
    enum GameObjects
    {
        BGMSlider,
        SFXSlider,
    }

    public override void Init()
    {
        base.Init();

        Bind<Slider>(typeof(GameObjects));

        Slider bgmSlider = Get<Slider>((int)GameObjects.BGMSlider);
        bgmSlider.onValueChanged.AddListener(delegate { BGMVolumeCtrl(); });
        bgmSlider.value = SoundManager.Instance.m_BGMSound;

        Slider sfxSlider = Get<Slider>((int)GameObjects.SFXSlider);
        sfxSlider.onValueChanged.AddListener(delegate { SFXVolumeCtrl(); });
        sfxSlider.value = SoundManager.Instance.m_SFXSound;
    }

    public void BGMVolumeCtrl()
    {
        float sound = Get<Slider>((int)GameObjects.BGMSlider).GetComponent<Slider>().value;

        if (sound <= -40)
            SoundManager.Instance.SetBGMSoundVolume(-80);
        else
            SoundManager.Instance.SetBGMSoundVolume(sound);
    }

    public void SFXVolumeCtrl()
    {
        float sound = Get<Slider>((int)GameObjects.SFXSlider).GetComponent<Slider>().value;

        if (sound <= -40)
            SoundManager.Instance.SetSFXSoundVolume(-80);
        else
            SoundManager.Instance.SetSFXSoundVolume(sound);
    }

    public void CloseSetting()
    {
        base.ClosePopupUI();
    }
}
