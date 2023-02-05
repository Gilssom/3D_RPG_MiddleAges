using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyScript;

public class DropBox : MonoBehaviour
{
    private Animator m_Anim;

    public LootDrop Loot;
    public int RandomDropCount;
    public float DropRange;
    
    public int m_Id;
    public GameObject m_AliveBox , m_DesBox;

    [SerializeField]
    private int m_Hp = 4;

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
        SetInfo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitBox")
            DestroyEvent();
    }

    void DestroyEvent()
    {
        if(m_Hp > 0)
        {
            m_Hp -= 1;
            m_Anim.SetTrigger("OnHit");

            if(m_Hp <= 0)
            {
                m_AliveBox.SetActive(false);
                m_DesBox.SetActive(true);
                m_Anim.Play("Bang");

                // 여기에 아이템 드랍 시스템 넣어야 함
                // Loot 컴포넌트를 각 몬스터 Id 별로 넣어줘야 하는 방법을 찾아야함
                // ex) Mutant Die -> Mutant Loot Table 을 얻어와야 함
                Loot.SpawnDrop(this.transform, RandomDropCount, DropRange);

                Invoke("DestroyObj", 2f);
            }
        }
    }

    void DestroyObj()
    {
        ResourcesManager.Instance.Destroy(gameObject);
    }

    void SetInfo()
    {
        Loot = ResourcesManager.Instance.Load<LootDrop>($"Data/Loot System/{m_Id}");
    }
}