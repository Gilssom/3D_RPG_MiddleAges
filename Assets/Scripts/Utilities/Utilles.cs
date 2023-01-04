using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. UI �ڵ�ȭ �κп��� ����� ��ƿ��Ƽ�� �ڵ���� �����ϴ� Ŭ����(��ũ��Ʈ)
public class Utilles
{
    // ���ϴ� ������Ʈ�� ������ �߰����ְ� ������ �����ش޶�� ��ƿ��Ƽ �Լ�
    public static T GetOrAddComponet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    // ���ӿ�����Ʈ ���� FindChild �Լ�
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    // �ֻ��� �θ� �ް� , �̸��� �ްų� Ÿ���� �ްų� , ��������� ã�� ���̳� -> (�ڽ��� �ڽĵ� ã�� ���̳� �� �Ǻ�)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object // ����Ƽ ������Ʈ�� ã�� ���̴�.
    {
        // �ֻ��� �θ� ������ null return
        if (go == null)
            return null;

        // ã�� ���� ������Ʈ�� <T> �� �־��༭ ã�� ���
        // 1. ��������� Ž���� �Ѵ�. ( �ڽ��� �ڽĵ� �Ǻ� )
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
        // 2. ��������� Ž���� ���Ѵ�. ( �ڽĸ� �Ǻ� )
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // �̸��� ����ְų�(-> Type) , ���� ���ϴ� �̸��̸� �ش� ��ü�� ��ȯ�ض�
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
