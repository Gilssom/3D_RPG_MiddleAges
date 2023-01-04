using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    // 원하는 컴포넌트를 없으면 추가해주고 있으면 참조해달라는 유틸리티 함수
    public static T GetOrAddComponet<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utilles.GetOrAddComponet<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
}
