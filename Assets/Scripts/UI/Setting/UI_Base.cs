using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // 각각의 타입에 해당되는 오브젝트들을 Objects[] 에 저장해준다.
    protected Dictionary<Type, UnityEngine.Object[]> m_Objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    // 부모클래스의 Start 에서 Init 을 호출하면,
    // 자식클래스의 Start 에서 Init 을 호출하는 함수를 제거해줘야 한다.
    void Start()
    {
        Init();
    }

    // enum 의 이름값을 자동적으로 찾아줘서 알아서 저장해줌.
    // Reflection 기능을 사용해 enum 값을 넘겨준다.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // 해당되는 이름의 enum 값들을 배열로 저장
        string[] names = Enum.GetNames(type);

        // Dictionary 에 추가
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_Objects.Add(typeof(T), objects);

        // 인스펙터 창에서 드래그 앤 드랍 하는 행위를 코드로 작성
        for (int i = 0; i < names.Length; i++)
        {
            // 게임 오브젝트는 어떻게 찾아야 할까
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utilles.FindChild(gameObject, names[i], true);

            // 해당되는 오브젝트를 어떻게 찾아야 할까
            else
                objects[i] = Utilles.FindChild<T>(gameObject, names[i], true);

            // 못찾았을 경우
            if (objects[i] == null)
                Debug.LogError($"Failed to bind({name[i]})");
        }
    }

    // 원하는 객체 꺼내서 사용하는 코드 - 유니티 상의 이름과 enum 의 이름들을 주의해서 사용하여 꺼내 쓰면 됨.
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (m_Objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[index] as T;
    }

    // ---Get 에 직접적으로 작성하기 귀찮을 수도 있으니 따로 만들어 준다----
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
