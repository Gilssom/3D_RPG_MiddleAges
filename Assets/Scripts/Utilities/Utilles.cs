using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. UI 자동화 부분에서 사용할 유틸리티성 코드들을 관리하는 클래스(스크립트)
public class Utilles
{
    // 원하는 컴포넌트를 없으면 추가해주고 있으면 참조해달라는 유틸리티 함수
    public static T GetOrAddComponet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    // 게임오브젝트 전용 FindChild 함수
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    // 최상위 부모를 받고 , 이름을 받거나 타입을 받거나 , 재귀적으로 찾을 것이냐 -> (자식의 자식도 찾을 것이냐 를 판별)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object // 유니티 오브젝트만 찾을 것이다.
    {
        // 최상위 부모가 없으면 null return
        if (go == null)
            return null;

        // 찾고 싶은 컴포넌트를 <T> 에 넣어줘서 찾는 방식
        // 1. 재귀적으로 탐색을 한다. ( 자식의 자식도 판별 )
        if(recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();

                    if (component != null)
                        return component;
                }
            }
        }
        // 2. 재귀적으로 탐색을 안한다. ( 자식만 판별 )
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // 이름이 비어있거나(-> Type) , 내가 원하는 이름이면 해당 객체를 반환해라
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
