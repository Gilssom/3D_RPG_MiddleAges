using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuickSlotCtrl : UI_Scene
{
    enum GameObjects
    {
        UI_Contents,
    }

    [SerializeField]
    private UI_Inven_Slot[] m_QuickSlots;
    [SerializeField]
    private Transform m_Parent;

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
}
