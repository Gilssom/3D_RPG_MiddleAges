using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
    }

    BaseInfo m_Stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        m_Stat = transform.parent.GetComponent<BaseInfo>();
    }

    void Update()
    {
        Transform parent = gameObject.transform.parent;

        // ��ġ�� �� ������Ʈ �ݶ��̴��� ���̸�ŭ ��������
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        // HP Bar �� ī�޶� �ٶ� �� �ֵ���
        transform.rotation = Camera.main.transform.rotation;

        // ������ ������Ʈ���� ������ �ִ� Stat �� ���� HP Bar ����
        float ratio = m_Stat.Hp / (float)m_Stat.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        if (ratio <= 0)
            ResourcesManager.Instance.Destroy(this.gameObject);

        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
