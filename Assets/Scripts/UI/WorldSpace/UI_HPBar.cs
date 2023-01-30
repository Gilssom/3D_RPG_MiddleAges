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

    enum Texts
    {
        TargetName,
    }

    BaseInfo m_Stat;
    EnemyInfo m_Enemy;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        m_Stat = transform.parent.GetComponent<BaseInfo>();
        m_Enemy = transform.parent.GetComponent<EnemyInfo>();
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
        SetName(m_Enemy.Name, m_Enemy.Level);
    }

    public void SetHpRatio(float ratio)
    {
        if (ratio <= 0)
            ResourcesManager.Instance.Destroy(this.gameObject);

        GetObject((int)GameObjects.HPBar).GetOrAddComponet<Slider>().value = ratio;
    }

    public void SetName(string name, int level)
    {
        GetText((int)Texts.TargetName).text = $"{name} <color=#D4BE3D>Lv.{level}</color> ";
    }
}
