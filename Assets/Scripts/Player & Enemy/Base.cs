using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    protected Vector3 m_InputDirection;
    public Vector3 m_CulatedDirection;
    protected Vector3 m_Gravity;

    protected Vector3 dir;

    #region #경사 체크 변수
    [Header("경사 지형 검사")]
    [SerializeField, Tooltip("캐릭터가 등반 가능한 최대 각도")]
    protected float m_MaxSlopeAngle;
    [SerializeField, Tooltip("경사 지형을 체크할 RayCast Pos")]
    protected Transform m_RaycastOrigin;

    protected const float m_RayDistance = 2f;
    protected RaycastHit m_SlopeHit;
    protected bool isOnSlope;
    #endregion

    #region #바닥 체크 변수
    [Header("지면 체크 검사")]
    [SerializeField, Tooltip("캐릭터가 땅에 붙어 있는지 확인하는 CheckBox Pos")]
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
