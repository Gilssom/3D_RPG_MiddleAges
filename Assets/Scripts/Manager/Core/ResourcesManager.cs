using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : SingletomManager<ResourcesManager>
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if(prefab == null)
        {
            Debug.LogWarning($"Failed to load prefab : {path}");
            return null;
        }

        // �ν��Ͻ� ���� �� Clone ���ڿ� �����ϱ�
        GameObject go = Object.Instantiate(prefab, parent);
        CloneDelete(go);
        //int index = go.name.IndexOf("(Clone)");
        //if (index > 0)
        //    go.name = go.name.Substring(0, index); // => 0��° ���� index��° ���� ¥����

        return go;
    }

    public void CloneDelete(GameObject go)
    {
        // �ν��Ͻ� ���� �� Clone ���ڿ� �����ϱ�
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index); // => 0��° ���� index��° ���� ¥����
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
