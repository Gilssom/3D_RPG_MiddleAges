using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : UI_Base
{
    enum GameObjects
    {
        Damage,
    }

    private float m_MoveSpeed;
    private float m_AlphaSpeed;
    private float m_DestroyTime;
    private Color alpha;
    private Text text;

    public int m_Damage;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        m_MoveSpeed = 0.5f;
        m_AlphaSpeed = 2f;
        m_DestroyTime = 1f;

        Invoke("DestroyObject", m_DestroyTime);

        Transform parent = gameObject.transform.parent;

        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        alpha = GetObject((int)GameObjects.Damage).GetOrAddComponet<Text>().color;
        text = GetObject((int)GameObjects.Damage).GetOrAddComponet<Text>();
    }

    void Update()
    {
        GetDamage(m_Damage);

        transform.rotation = Camera.main.transform.rotation;

        transform.Translate(new Vector3(0, m_MoveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * m_AlphaSpeed);

        text.color = alpha;
    }

    void GetDamage(int damage)
    {
        text.text = $"-{damage}";
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
