using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    // UI_Popup 을 상속받은 애들에게서 해당 함수를 호출하면 알아서 Close
    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
