using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    // UI_Popup �� ��ӹ��� �ֵ鿡�Լ� �ش� �Լ��� ȣ���ϸ� �˾Ƽ� Close
    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
