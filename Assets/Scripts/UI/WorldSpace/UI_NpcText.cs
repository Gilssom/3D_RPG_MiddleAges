using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NpcText : UI_Base
{
    public override void Init()
    {

    }

    void Update()
    {
        Transform parent = gameObject.transform.parent;

        // 위치를 각 오브젝트 콜라이더의 높이만큼 설정해줌
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        // HP Bar 가 카메라를 바라볼 수 있도록
        transform.rotation = Camera.main.transform.rotation;
    }
}
