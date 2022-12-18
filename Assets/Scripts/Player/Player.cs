using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System �� ����Ѵ�.
using UnityEngine.InputSystem.Interactions;
using CharacterController;
using DG.Tweening;

// Ư�� ������Ʈ�� �ڵ������� �߰�����. ���� ���� ���� ��ũ��Ʈ �߰� ����
[RequireComponent(typeof(PlayerInfo))]
public class Player : MonoBehaviour
{
    public PlayerInfo playerInfo         { get; private set; }
    public Vector3 m_InputDirection      { get; private set; }
    public Vector3 m_CulatedDirection    { get; private set; }
    public Vector3 m_Gravity             { get; private set; }

    public enum PlayerState
    {
        MOVE,   // Idle �� Move
        DASH,   // Dash
        NDASH,  // N-Dash
    }

    protected PlayerState m_PlayerState;

    private Transform m_Chracter;
    [SerializeField]
    private Transform m_Camera;
    //private Animator m_Anim;
    //private Rigidbody m_Rigid;

    //------- New Input System --------
    public Vector3 dir              { get; private set; }
    public Vector3 m_LookForward    { get; private set; }

    //------- Player Slope (����) �̵� ���� --------
    #region #��� üũ ����
    [Header("��� ���� �˻�")]
    [SerializeField, Tooltip("ĳ���Ͱ� ��� ������ �ִ� ����")]
    private float m_MaxSlopeAngle;
    [SerializeField, Tooltip("��� ������ üũ�� RayCast Pos")]
    private Transform m_RaycastOrigin;

    private const float m_RayDistance = 2f;
    private RaycastHit m_SlopeHit;
    private bool isOnSlope;
    #endregion

    #region #�ٴ� üũ ����
    [Header("���� üũ �˻�")]
    [SerializeField, Tooltip("ĳ���Ͱ� ���� �پ� �ִ��� Ȯ���ϴ� CheckBox Pos")]
    private Transform m_GroundCheck;
    private int m_GroundLayer;
    private bool isGrounded;
    #endregion

    //------- Player Dash ���� -------

    #region #�뽬�� ����
    [Header("�뽬�� �ɼ�")]
    [SerializeField, Tooltip("Dash �Ŀ�")]
    protected float m_DashPower;
    [SerializeField, Tooltip("Dash �ձ����� ��� �ð�")]
    protected float m_DashRollTime;
    [SerializeField, Tooltip("Dash ���Է� �ð�")]
    protected float m_DashReInputTime;
    [SerializeField, Tooltip("Dash ���� �ð�")]
    protected float m_DashTetanyTime;
    [SerializeField, Tooltip("Dash ���� ���ð�")]
    protected float m_DashCoolTime;

    private WaitForSeconds Dash_Roll_Time;
    private WaitForSeconds Dash_ReInput_Time;
    private WaitForSeconds Dash_Tetany_Time;
    private Coroutine m_DashCoroutine;
    private Coroutine m_DashCoolTimeCoroutine;
    private int m_CurDashCount;
    #endregion

    public float HAxis { get; private set; }
    public float VAxis { get; private set; }
    public bool LSDown { get; private set; }

    void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        m_Chracter = GetComponent<Transform>();
        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground");

        Dash_Roll_Time = new WaitForSeconds(m_DashRollTime);
        Dash_ReInput_Time = new WaitForSeconds(m_DashReInputTime);
        Dash_Tetany_Time = new WaitForSeconds(m_DashTetanyTime);

        // Test
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //-------New Dash System ��ũ��Ʈ --------
    protected Vector3 GetDirection(float curMoveSpeed)
    {
        Vector2 moveInput = new Vector2(HAxis, VAxis);
        Vector3 lookForward = new Vector3(m_Camera.forward.x, 0f, m_Camera.forward.z).normalized;
        Vector3 lookRight = new Vector3(m_Camera.right.x, 0f, m_Camera.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        isOnSlope = IsOnSlope();
        isGrounded = IsGrounded();

        Vector3 m_CulatedDirection = NextFrameGroundAngle(curMoveSpeed) < m_MaxSlopeAngle ? moveDir : Vector3.zero;

        m_CulatedDirection = (isGrounded && isOnSlope) ? AdjustDirectionToSlope(m_CulatedDirection) : m_CulatedDirection.normalized;

        m_LookForward = lookForward;

        return m_CulatedDirection;
    }

    protected void CtrlGravity()
    {
        if (isGrounded && isOnSlope)
        {
            playerInfo.m_Rigid.useGravity = false;
            return;
        }

        playerInfo.m_Rigid.useGravity = true;
    }

    void Update()
    {
        m_CulatedDirection = GetDirection(playerInfo.MoveSpeed);
        CtrlGravity();
    }

    //-------New Input System ��ũ��Ʈ ���� ���---------

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        m_InputDirection = new Vector3(input.x, 0, input.y);
        DOTween.To(() => HAxis, x => HAxis = x, input.x, 0.3f);
        DOTween.To(() => VAxis, y => VAxis = y, input.y, 0.3f);
    }

    #region #�÷��̾� �뽬�� �ý���
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isAbleDash = m_PlayerState != PlayerState.DASH && m_CurDashCount < playerInfo.DashCount && isGrounded;

            if (isAbleDash)
            {
                m_PlayerState = PlayerState.DASH;
                m_CurDashCount++;

                if (m_DashCoroutine != null && m_DashCoolTimeCoroutine != null)
                {
                    StopCoroutine(m_DashCoroutine);
                    StopCoroutine(m_DashCoolTimeCoroutine);
                }

                m_DashCoroutine = StartCoroutine(DashCoroutine());
            }
        }
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 DashDir = (m_CulatedDirection == Vector3.zero) ? m_Camera.forward : m_CulatedDirection;
        Debug.Log(DashDir);

        playerInfo.m_Anim.SetBool("isDashing", true);
        playerInfo.m_Anim.SetTrigger("Dash");

        //playerInfo.m_Rigid.AddForce(transform.position += (DashDir * m_DashPower * Time.deltaTime));
        playerInfo.m_Rigid.velocity = DashDir * m_DashPower;

        yield return Dash_Roll_Time;
        m_PlayerState = (playerInfo.DashCount > 1 && m_CurDashCount < playerInfo.DashCount) ? PlayerState.NDASH : PlayerState.DASH;

        yield return Dash_ReInput_Time;
        playerInfo.m_Anim.SetBool("isDashing", false);
        playerInfo.m_Rigid.velocity = Vector3.zero;

        yield return Dash_Tetany_Time;
        m_PlayerState = PlayerState.MOVE;

        m_DashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());

    }

    private IEnumerator DashCoolTimeCoroutine()
    {
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;
            if (curTime >= m_DashCoolTime)
                break;
            yield return null;
        }

        if (m_CurDashCount == playerInfo.DashCount)
            m_CurDashCount = 0;
    }
    #endregion

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LSDown = true;
        }
        else if (context.canceled)
        {
            LSDown = false;
        }
    }

    public void LookAt(Vector3 lookForward)
    {
        if (m_InputDirection != Vector3.zero)
        {
            m_Chracter.forward = lookForward;
        }
    }

    #region #�÷��̾� ��絵 �� �ٴ� üũ
    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out m_SlopeHit, m_RayDistance, m_GroundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, m_SlopeHit.normal);
            return angle != 0f && angle < m_MaxSlopeAngle;
        }

        return false;
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, m_SlopeHit.normal).normalized;
    }

    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
        return Physics.CheckBox(m_GroundCheck.position, boxSize, Quaternion.identity, m_GroundLayer);
    }

    // isGournded Test Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
        Gizmos.DrawWireCube(m_GroundCheck.position, boxSize);
    }

    private float NextFrameGroundAngle(float moveSpeed)
    {
        var nextFramePlayerPos = m_RaycastOrigin.position + dir * moveSpeed * Time.deltaTime;

        if (Physics.Raycast(nextFramePlayerPos, Vector3.down, out RaycastHit hitInfo, m_RayDistance, m_GroundLayer))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }

        return 0f;
    }
    #endregion
}