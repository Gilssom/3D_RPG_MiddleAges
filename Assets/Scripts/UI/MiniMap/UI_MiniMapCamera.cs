using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MiniMapCamera : MonoBehaviour
{
    private Transform m_Transform;

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    private void Update()
    {
        m_Transform.position = new Vector3(BaseInfo.playerInfo.m_Player.transform.position.x, 10, BaseInfo.playerInfo.m_Player.transform.position.z);
    }
}
