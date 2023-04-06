using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsInfo
{
    // Object 들의 정보들을 관리하는 Class
    public string m_ObjectName;
    public GameObject m_Prefab;
    public int m_Count;
}

public class ObjectPoolManager : SingletomManager<ObjectPoolManager>
{
    /// <summary>
    /// 0 : 은하계의 레기오로스 히트 이펙트
    /// 1 : 태양계의 레기오로스 히트 이펙트
    /// 2 : 미궁의 레기오로스 히트 이펙트
    /// </summary>

    public GameObject Pooling
    {
        get
        {
            GameObject root = GameObject.Find("@@##_Object_Pooling");
            if (root == null)
                root = new GameObject { name = "@@##_Object_Pooling" };
            return root;
        }
    }

    [SerializeField]
    ObjectsInfo[] m_ObjectInfos = null;

    public List<Queue<GameObject>> m_ObjectPoolList;

    void Start()
    {
        m_ObjectPoolList = new List<Queue<GameObject>>();

        if (m_ObjectInfos != null)
            for (int i = 0; i < m_ObjectInfos.Length; i++)
            {
                m_ObjectPoolList.Add(InsertQueue(m_ObjectInfos[i]));
            }
    }

    Queue<GameObject> InsertQueue(ObjectsInfo prefab_objectInfo)
    {
        Queue<GameObject> m_Queue = new Queue<GameObject>();

        for (int i = 0; i < prefab_objectInfo.m_Count; i++)
        {
            GameObject objectClone = Instantiate(prefab_objectInfo.m_Prefab) as GameObject;

            objectClone.SetActive(false);
            objectClone.transform.SetParent(Pooling.transform);

            int index = objectClone.name.IndexOf("(Clone)");
            if (index > 0)
                objectClone.name = objectClone.name.Substring(0, index);

            m_Queue.Enqueue(objectClone);
        }

        return m_Queue;
    }

    public IEnumerator DestroyObj(float Seconds, int PoolNumber, GameObject Object)
    {
        yield return new WaitForSeconds(Seconds);
        m_ObjectPoolList[PoolNumber].Enqueue(Object);
        Object.SetActive(false);
    }
}
