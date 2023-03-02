using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_Slot : UI_Base
{
    enum Buttons
    {
        UI_Skill_Slot,
    }

    public Item m_Item;
    bool isUsed;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        SetEvent();

        InvokeRepeating("CheckLevel", 0, 1);
    }

    private void OnEnable()
    {
        if (isUsed)
            CancelInvoke("CheckLevel");
    }

    // UI EventHandler 등록
    void SetEvent()
    {
        // 아이템 Enter 이벤트
        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} 정보!"); 
            if (m_Item != null)
                InventoryManager.Instance.ShowToolTip(m_Item, GetComponent<RectTransform>(), 0);
        }
        , Defines.UIEvent.Enter);

        // 아이템 Exit 이벤트
        gameObject.BindEvent((PointerEventData) =>
        {
            //Debug.Log($"{m_Item.m_ItemName} 정보 안보기"); 
            InventoryManager.Instance.HideToolTip();
        }
        , Defines.UIEvent.Exit);
    }

    void CheckLevel()
    {
        if (m_Item != null)
        {
            if (BaseInfo.playerInfo.Level < m_Item.m_UsedLevel)
            {
                GetButton((int)Buttons.UI_Skill_Slot).interactable = false;
            }
            else
            {
                GetButton((int)Buttons.UI_Skill_Slot).interactable = true;
                isUsed = true;
            }
        }       
    }
}
