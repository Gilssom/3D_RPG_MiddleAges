using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // ������ Ÿ�Կ� �ش�Ǵ� ������Ʈ���� Objects[] �� �������ش�.
    protected Dictionary<Type, UnityEngine.Object[]> m_Objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    // �θ�Ŭ������ Start ���� Init �� ȣ���ϸ�,
    // �ڽ�Ŭ������ Start ���� Init �� ȣ���ϴ� �Լ��� ��������� �Ѵ�.
    void Start()
    {
        Init();
    }

    // enum �� �̸����� �ڵ������� ã���༭ �˾Ƽ� ��������.
    // Reflection ����� ����� enum ���� �Ѱ��ش�.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // �ش�Ǵ� �̸��� enum ������ �迭�� ����
        string[] names = Enum.GetNames(type);

        // Dictionary �� �߰�
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_Objects.Add(typeof(T), objects);

        // �ν����� â���� �巡�� �� ��� �ϴ� ������ �ڵ�� �ۼ�
        for (int i = 0; i < names.Length; i++)
        {
            // ���� ������Ʈ�� ��� ã�ƾ� �ұ�
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utilles.FindChild(gameObject, names[i], true);

            // �ش�Ǵ� ������Ʈ�� ��� ã�ƾ� �ұ�
            else
                objects[i] = Utilles.FindChild<T>(gameObject, names[i], true);

            // ��ã���� ���
            if (objects[i] == null)
                Debug.LogError($"Failed to bind({name[i]})");
        }
    }

    // ���ϴ� ��ü ������ ����ϴ� �ڵ� - ����Ƽ ���� �̸��� enum �� �̸����� �����ؼ� ����Ͽ� ���� ���� ��.
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (m_Objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[index] as T;
    }

    // ---Get �� ���������� �ۼ��ϱ� ������ ���� ������ ���� ����� �ش�----
    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int index)         { return Get<Text>(index); }
    protected Button GetButton(int index)     { return Get<Button>(index); }
    protected Image GetImage(int index)       { return Get<Image>(index); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_EvnetHandler evt = Utilles.GetOrAddComponet<UI_EvnetHandler>(go);

        switch (type)
        {
            case Defines.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Defines.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Defines.UIEvent.Enter:
                evt.OnEnterHandler -= action;
                evt.OnEnterHandler += action;
                break;
        }
    }
}
