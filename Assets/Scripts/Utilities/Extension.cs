using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    // ���ϴ� ������Ʈ�� ������ �߰����ְ� ������ �����ش޶�� ��ƿ��Ƽ �Լ�
    public static T GetOrAddComponet<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utilles.GetOrAddComponet<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    // �ش� ���ӿ�����Ʈ�� �ְ� , Ȱ��ȭ�� ���ִ���
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
