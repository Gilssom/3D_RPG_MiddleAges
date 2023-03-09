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

        // ��ġ�� �� ������Ʈ �ݶ��̴��� ���̸�ŭ ��������
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        // HP Bar �� ī�޶� �ٶ� �� �ֵ���
        transform.rotation = Camera.main.transform.rotation;
    }
}
