using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WindowMove : UI_Base
{
    [SerializeField]
    private Transform m_TargetTr; // 이동될 UI

    private Vector2 m_BeginPoint;
    private Vector2 m_MoveBegin;

    public override void Init()
    {
        // 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화
        if (m_TargetTr == null)
            m_TargetTr = GetComponent<Transform>();

        SetEvent();
    }
    
    void SetEvent()
    {
        // 아이템 드래그 이벤트
        gameObject.BindEvent((PointerEventData) =>
        {
            m_BeginPoint = m_TargetTr.position;
            m_MoveBegin = PointerEventData.position;
        }
        , Defines.UIEvent.BeginDrag);

        gameObject.BindEvent((PointerEventData) =>
        {
            m_TargetTr.position = m_BeginPoint + (PointerEventData.position - m_MoveBegin);
        }
        , Defines.UIEvent.Drag);
    }
}
