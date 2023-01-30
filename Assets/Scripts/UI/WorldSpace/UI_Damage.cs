using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : UI_Base
{
    enum Texts
    {
        Damage,
    }

    private float m_MoveSpeed;
    private float m_AlphaSpeed;
    private float m_DestroyTime;
    private Color alpha;
    private Text text;

    public int m_Damage;
    public bool m_Critical;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));

        m_MoveSpeed = 0.5f;
        m_AlphaSpeed = 2f;
        m_DestroyTime = 1f;

        Invoke("DestroyObject", m_DestroyTime);

        Transform parent = gameObject.transform.parent;

        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        text = GetText((int)Texts.Damage);
        alpha = text.color;
    }

    void Update()
    {
        GetDamage(m_Damage, m_Critical);

        transform.rotation = Camera.main.transform.rotation;

        transform.Translate(new Vector3(0, m_MoveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * m_AlphaSpeed);

        text.color = alpha;
    }

    void GetDamage(int damage, bool critical)
    {
        if(critical)
        {
            text.fontSize = 50;
            // new Color 또한 가비지가 안생김. new Vector 와 동일하다.
            alpha = new Color(1f, 225 / 250f, 74 / 250f);
        }

        text.text = $"-{damage}";
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
