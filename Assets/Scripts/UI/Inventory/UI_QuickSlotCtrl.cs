using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuickSlotCtrl : UI_Scene
{
    enum GameObjects
    {
        UI_Contents,
    }

    [SerializeField]
    private UI_Inven_Slot[] m_QuickSlots;
    [SerializeField]
    private Image[] m_QuickSlotCoolTime = new Image[4];
    [SerializeField]
    private Transform m_Parent;

    [SerializeField]
    private float m_CoolTime = 5, m_CurCoolTime;
    private bool isCoolTime;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        m_Parent = GetObject((int)GameObjects.UI_Contents).transform;

        m_QuickSlots = m_Parent.GetComponentsInChildren<UI_Inven_Slot>();

        for (int i = 0; i < m_QuickSlots.Length; i++)
        {
            m_QuickSlots[i].m_QuickSlotBaseRect = m_Parent.GetComponent<RectTransform>();
        }

        InventoryManager.Instance.m_QuickSlotBaseRect = GetObject((int)GameObjects.UI_Contents).GetComponent<RectTransform>();
    }

    void Update()
    {
        SetCoolTime();
    }

    void SetCoolTime()
    {
        if (isCoolTime)
        {
            m_CurCoolTime -= Time.deltaTime;

            for (int i = 0; i < m_QuickSlotCoolTime.Length; i++)
            {
                m_QuickSlotCoolTime[i].fillAmount = m_CurCoolTime / m_CoolTime;
            }

            if (m_CurCoolTime <= 0)
            {
                isCoolTime = false;
            }
        }
    }

    void ResetCoolTime()
    {
        m_CurCoolTime = m_CoolTime;
        isCoolTime = true;
    }

    public void EatItem(int selectNum)
    {
        if (m_QuickSlots[selectNum].m_Item != null && !isCoolTime)
        {
            if (m_QuickSlots[selectNum].m_Item.m_UsedType == Item.UsedType.Potion && (ItemEffectDataBase.Instance.m_Player.playerInfo.Hp == ItemEffectDataBase.Instance.m_Player.playerInfo.MaxHp))
            {
                Debug.LogWarning("플레이어의 체력이 가득 차있습니다.");
                return;
            }

            ResetCoolTime();
            ItemEffectDataBase.Instance.UseItem(m_QuickSlots[selectNum].m_Item);
            m_QuickSlots[selectNum].SetSlotCount(-1);
        }
    }
}
