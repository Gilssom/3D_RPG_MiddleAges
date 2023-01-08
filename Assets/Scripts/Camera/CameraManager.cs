using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook m_FreeLook;
    private GameObject m_Player = null;

    public void SetPlayer(GameObject player)
    {
        m_Player = player;

        if (m_Player.IsValid() == false)
            return;

        m_FreeLook.Follow = m_Player.transform;
        m_FreeLook.LookAt = m_Player.transform;
    }

    void LateUpdate()
    {
        if (m_Player.IsValid() == false)
            return;

        Vector3 dir = (m_Player.transform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, Mathf.Infinity,
            1 << LayerMask.NameToLayer("EnviromentObject"));

        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }
}
