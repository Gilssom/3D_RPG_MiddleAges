using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WindowMove : UI_Base
{
    [SerializeField]
    private Transform m_TargetTr; // �̵��� UI

    private Vector2 m_BeginPoint;
    private Vector2 m_MoveBegin;

    public override void Init()
    {
        // �̵� ��� UI�� �������� ���� ���, �ڵ����� �θ�� �ʱ�ȭ
        if (m_TargetTr == null)
            m_TargetTr = GetComponent<Transform>();

        SetEvent();
    }
    
    void SetEvent()
    {
        // ������ �巡�� �̺�Ʈ
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
