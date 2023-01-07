using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System 을 사용한다.
using UnityEngine.InputSystem.Interactions;
using CharacterController;
using DG.Tweening;

// 특정 컴포넌트를 자동적으로 추가해줌. 추후 스탯 관리 스크립트 추가 예정
[RequireComponent(typeof(PlayerInfo))]
public class Player : Base
{
    public PlayerInfo playerInfo         { get; private set; }

    protected DashState m_DashState;

    private Transform m_Chracter;
    [SerializeField]
    private Transform m_Camera;

    //------- New Input System --------
    public Vector3 m_LookForward    { get; private set; }

    //------- Player Dash 구현 -------

    #region #대쉬기 변수
    private WaitForSeconds Dash_Roll_Time;
    private WaitForSeconds Dash_ReInput_Time;
    private WaitForSeconds Dash_Tetany_Time;
    private Coroutine m_DashCoroutine;
    private Coroutine m_DashCoolTimeCoroutine;
    #endregion

    public float HAxis { get; private set; }
    public float VAxis { get; private set; }
    public bool LSDown { get; private set; }
    public bool isNormalAttack { get; private set; }
    public bool isChargeAttack { get; private set; }

    public override void Init()
    {
        WorldObjectType = Defines.WorldObject.Player;
        playerInfo = GetComponent<PlayerInfo>();
        m_Chracter = GetComponent<Transform>();
        m_Camera = Camera.main.transform;
        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground");

        Dash_Roll_Time = new WaitForSeconds(playerInfo.m_DashRollTime);
        Dash_ReInput_Time = new WaitForSeconds(playerInfo.m_DashReInputTime);
        Dash_Tetany_Time = new WaitForSeconds(playerInfo.m_DashTetanyTime);

        m_DashState = playerInfo.stateMachine.GetState(StateName.DASH) as DashState;

        // Test
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            UIManager.Instance.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    #region #기본 시스템
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

    protected override void CtrlGravity()
    {
        m_CulatedDirection = GetDirection(playerInfo.MoveSpeed);

        if (!DashState.m_IsDash)
        {
            playerInfo.m_Rigid.velocity = Vector3.zero;
            playerInfo.m_Rigid.angularVelocity = Vector3.zero;
        }

        if (isGrounded && isOnSlope)
        {
            playerInfo.m_Rigid.useGravity = false;
            return;
        }

        playerInfo.m_Rigid.useGravity = true;
    }
    #endregion

    #region #UI 테스트
    public void InventoryUIOpen(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!UIManager.Instance.isInvenOpen)
            {
                UIManager.Instance.ShowPopupUI<UI_Inven>();
            }
            else
            {
                UIManager.Instance.isInvenOpen = false;
                UIManager.Instance.ClosePopupUI();
            }
        }
    }

    public void TestButtonClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInfo.Exp += 20;
        }
    }
    #endregion

    #region #플레이어 이동 시스템
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        m_InputDirection = new Vector3(input.x, 0, input.y);
        DOTween.To(() => HAxis, x => HAxis = x, input.x, 0.3f);
        DOTween.To(() => VAxis, y => VAxis = y, input.y, 0.3f);
    }

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
    #endregion

    #region #플레이어 대쉬기 시스템
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isabledash = !DashState.m_IsDash && m_DashState.m_CurDashCount < playerInfo.DashCount && isGrounded;
            Debug.LogWarning(isabledash);

            if (isabledash)
            {
                if(AttackState.isAttack)
                {
                    return;
                }

                m_DashState.m_CurDashCount++;
                playerInfo.stateMachine.ChangeState(StateName.DASH);

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
        yield return Dash_Roll_Time;
        if (playerInfo.DashCount > 1 && m_DashState.m_CurDashCount < playerInfo.DashCount)
        {
            playerInfo.stateMachine.ChangeState(StateName.MOVE);
        }
        yield return Dash_ReInput_Time;
        m_DashState.OnExitState();

        yield return Dash_Tetany_Time;
        playerInfo.stateMachine.ChangeState(StateName.MOVE);

        m_DashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());

    }

    private IEnumerator DashCoolTimeCoroutine()
    {
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;
            if (curTime >= m_DashState.m_DashCoolTime)
                break;
            yield return null;
        }

        if (m_DashState.m_CurDashCount == playerInfo.DashCount)
            m_DashState.m_CurDashCount = 0;
    }
    #endregion

    #region #플레이어 공격 시스템
    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(context.interaction is HoldInteraction)       // 차지 공격
            {
                if (DashState.m_IsDash)
                {
                    return;
                }

                AttackState.m_AttackName = AttackState.AttackName.CHARGE;
                playerInfo.stateMachine.ChangeState(StateName.ATTACK);
            }

            else if(context.interaction is PressInteraction) // 일반 공격
            {
                AttackState.m_AttackName = AttackState.AttackName.ATTACK;

                if (DashState.m_IsDash)
                {
                    return;
                }

                bool isAbleAttack = !AttackState.isAttack && (playerInfo.m_WeaponManager.m_Weapon.m_ComboCount < 3);

                if(isAbleAttack)
                {
                    playerInfo.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
    }

    public void OnClickRightMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is PressInteraction)       // 발차기 공격
            {
                if (DashState.m_IsDash && AttackState.isAttack)
                {
                    return;
                }

                AttackState.m_AttackName = AttackState.AttackName.KICK;
                playerInfo.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }
    #endregion

    #region #플레이어 방향
    public void LookAt(Vector3 lookForward)
    {
        if (m_InputDirection != Vector3.zero)
        {
            m_Chracter.forward = lookForward;
        }
    }
    #endregion

    #region #플레이어 피격 이벤트
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHitBox")
            OnHitEvent(other.transform.parent.gameObject.GetComponent<EnemyInfo>());
    }

    void OnHitEvent(EnemyInfo enemy)
    {
        if (playerInfo.Hp > 0)
        {
            int damage = UnityEngine.Random.Range(enemy.Attack - playerInfo.Defense, enemy.Attack + 1);
            Debug.Log($"적에게 입은 피해량 : {damage} !!");
            playerInfo.Hp -= damage;

            // 플레이어 사망
            if (playerInfo.Hp <= 0)
                GameManager.Instance.Despawn(gameObject);
                //playerInfo.stateMachine.ChangeState(StateName.DIE);
        }
    }
    #endregion

    #region #플레이어 경사도 및 바닥 체크
    protected bool IsOnSlope()
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