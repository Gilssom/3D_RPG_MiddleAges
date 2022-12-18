using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 dir = (PlayerInfo.Instance.transform.position - transform.position).normalized;
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
