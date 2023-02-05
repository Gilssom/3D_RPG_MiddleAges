using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item m_Item;

    private SphereCollider m_Coll;

    private void Start()
    {
        m_Coll = GetComponent<SphereCollider>();
        Invoke("PickUpEnable", 1.5f);
    }

    void PickUpEnable()
    {
        m_Coll.enabled = true;
    }
}
