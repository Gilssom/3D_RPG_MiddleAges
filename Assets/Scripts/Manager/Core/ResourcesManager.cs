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

        // 인스턴스 생성 시 Clone 문자열 제거하기
        GameObject go = Object.Instantiate(prefab, parent);
        CloneDelete(go);
        //int index = go.name.IndexOf("(Clone)");
        //if (index > 0)
        //    go.name = go.name.Substring(0, index); // => 0번째 부터 index번째 까지 짜르기

        return go;
    }

    public void CloneDelete(GameObject go)
    {
        // 인스턴스 생성 시 Clone 문자열 제거하기
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index); // => 0번째 부터 index번째 까지 짜르기
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
