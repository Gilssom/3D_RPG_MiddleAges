using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    protected Vector3 m_InputDirection;
    public Vector3 m_CulatedDirection;
    protected Vector3 m_Gravity;

    protected Vector3 dir;

    #region #��� üũ ����
    [Header("��� ���� �˻�")]
    [SerializeField, Tooltip("ĳ���Ͱ� ��� ������ �ִ� ����")]
    protected float m_MaxSlopeAngle;
    [SerializeField, Tooltip("��� ������ üũ�� RayCast Pos")]
    protected Transform m_RaycastOrigin;

    protected const float m_RayDistance = 2f;
    protected RaycastHit m_SlopeHit;
    protected bool isOnSlope;
    #endregion

    #region #�ٴ� üũ ����
    [Header("���� üũ �˻�")]
    [SerializeField, Tooltip("ĳ���Ͱ� ���� �پ� �ִ��� Ȯ���ϴ� CheckBox Pos")]
    protected Transform m_GroundCheck;
    protected int m_GroundLayer;
    protected bool isGrounded;
    #endregion

    public Defines.WorldObject WorldObjectType { get; protected set; } = Defines.WorldObject.Unknown;

    private void Start() { Init(); }
    public abstract void Init();

    void Update() { CtrlGravity(); }
    protected virtual void CtrlGravity() { }
}
