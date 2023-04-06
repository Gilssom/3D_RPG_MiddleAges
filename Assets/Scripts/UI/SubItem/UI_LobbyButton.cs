using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LobbyButton : UI_Base
{
    public override void Init()
    {
        SetEvent();
    }

    // UI EventHandler µî·Ï
    protected void SetEvent()
    {
        gameObject.BindEvent((PointerEventData) =>
        {
            SoundManager.Instance.Play("UI/Mouse");
        }
        , Defines.UIEvent.Enter);

        gameObject.BindEvent((PointerEventData) =>
        {
            SoundManager.Instance.Play("UI/Player Info Viewer");
        }
        , Defines.UIEvent.Click);
    }
}
