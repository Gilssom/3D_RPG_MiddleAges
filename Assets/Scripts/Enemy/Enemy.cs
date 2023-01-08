using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterController;
using DG.Tweening;

[RequireComponent(typeof(EnemyInfo))]
public class Enemy : Base
{
    public EnemyInfo enemyInfo { get; private set; }

    private Transform m_Chracter;
    [SerializeField]
    private GameObject m_Target;
    private Vector3 m_Pos;

    private WaitForSeconds m_OnDamageColor;

    public override void Init()
    {
        WorldObjectType = Defines.WorldObject.Monster;
        enemyInfo = GetComponent<EnemyInfo>();
        m_Chracter = GetComponent<Transform>();
        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground");
        m_OnDamageColor = new WaitForSeconds(0.2f);

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            UIManager.Instance.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    #region #기본 이동 시스템
    /*protected Vector3 GetDirection(float curMoveSpeed)
    {
        isOnSlope = IsOnSlope();
        isGrounded = IsGrounded();

        Vector3 m_CulatedDirection = NextFrameGroundAngle(curMoveSpeed) < m_MaxSlopeAngle ? moveDir : Vector3.zero;

        m_CulatedDirection = (isGrounded && isOnSlope) ? AdjustDirectionToSlope(m_CulatedDirection) : m_CulatedDirection.normalized;

        return m_CulatedDirection;
    }

    protected override void CtrlGravity()
    {
        m_CulatedDirection = GetDirection(enemyInfo.MoveSpeed);

        if (m_InputDirection.magnitude == 0)
        {
            enemyInfo.m_Rigid.velocity = Vector3.zero;
        }

        if (isGrounded && isOnSlope)
        {
            enemyInfo.m_Rigid.useGravity = false;
            return;
        }

        enemyInfo.m_Rigid.useGravity = true;
    }*/

    public void Move()
    {
        MoveStop(false);
        NavMeshAgent navmesh = gameObject.GetOrAddComponet<NavMeshAgent>();
        navmesh.SetDestination(m_Target.transform.position);
        navmesh.speed = enemyInfo.MoveSpeed;

        LookAt();
    }

    public void MoveStop(bool isStop)
    {
        NavMeshAgent navmesh = gameObject.GetOrAddComponet<NavMeshAgent>();
        navmesh.isStopped = isStop;
        enemyInfo.m_Rigid.velocity = Vector3.zero;
        enemyInfo.m_Rigid.angularVelocity = Vector3.zero;
    }
    #endregion


    //------ AI 시스템 ---------
    /// <summary>
    /// 1. 플레이어를 감지하게 되면 EnemyInfo 의 CheckPlayer 를 true 로 변경
    /// >> Target 이 Player 로 잡히게 된다.
    /// => State 를 Move 로 전환
    /// 
    /// 2. 플레이어가 공격 범위 내에 들어오게 되면 EnemyInfo 의 ReadyAttack 을 true 로 변경
    /// >> Target 은 그대로 Player .
    /// => State 를 Attack 으로 전환
    /// 
    /// 3. 플레이어가 공격 범위 내를 벗어나면 ReadyAttack 을 false,
    ///    => State 를 Move 로 전환 >> 1번 상태로 전환함
    ///    플레이어가 감지 범위 내를 벗어나면 CheckPlayer 를 false.
    ///    => State 를 Idle 로 전환
    ///    
    /// 공격을 하게 되면 ReadyAttack 을 끄고 4초간의 시간 이후 다시 true 로 하여 Attack
    /// >> Attack 이후 ReadyAttack 을 false , 
    ///    false 시간 동안 Idle 로 전환 >> 3번 상태로 전환
    /// 
    /// </summary>
    public void PlayerCheck()
    {
        // 플레이어 체크
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        m_Pos = transform.position;
        float distance = (player.transform.position - transform.position).magnitude;

        // 1번 공격 전환
        if (distance <= enemyInfo.AttackRange)
        {
            //EnemyAttackState.isAttack == false && 
            if (enemyInfo.CheckPlayer && enemyInfo.ReadyAttack)
            {
                Debug.LogWarning("공격 상태로 전환");
                // Attack 로 변경
                enemyInfo.stateMachine.ChangeState(StateName.ATTACK);
                return;
            }
        }
        // 2번 추적 전환
        else if (distance <= enemyInfo.ScanRange)
        {
            Debug.LogWarning("추적 상태로 전환");
            enemyInfo.CheckPlayer = true;
            m_Target = player;
            // Move 로 변경
            enemyInfo.stateMachine.ChangeState(StateName.MOVE);
            return;            
        }
        // 3번 기본 자세 전환
        else
        {
            Debug.LogWarning("기본 상태로 전환");
            enemyInfo.CheckPlayer = false;
            enemyInfo.ReadyAttack = false;
            m_Target = null;
            // Idle 로 변경
            enemyInfo.stateMachine.ChangeState(StateName.IDLE);
            return;
        }
    }

    #region #몬스터 공격 시스템
    public IEnumerator AttackCoroutine()
    {
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;
            if (curTime >= enemyInfo.AttackCoolTime)
                break;
            yield return null;
        }

        enemyInfo.ReadyAttack = true;
    }
    #endregion

    #region #몬스터 방향
    public void LookAt()
    {
        if(m_Target == null)
        {
            return;
        }

        m_Chracter.LookAt(m_Target.transform.position);
    }
    #endregion

    #region #몬스터 피격 이벤트
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitBox")
            OnHitEvent(other.transform.parent.gameObject.GetComponent<PlayerInfo>());
    }

    void OnHitEvent(PlayerInfo player)
    {
        if(enemyInfo.Hp > 0)
        {
            int damage = Random.Range(player.Attack - enemyInfo.Defense, player.Attack + 1);
            Debug.Log($"적에게 가한 피해량 : {damage} !!");
            enemyInfo.Hp -= damage;
            StartCoroutine(OnDamageEvent());

            // 몬스터 사망
            if(enemyInfo.Hp <= 0)
                enemyInfo.stateMachine.ChangeState(StateName.DIE);
        }
    }

    IEnumerator OnDamageEvent()
    {
        ShakeCamera.Instance.CameraShake(3f, 0.2f);
        HitEffect();
        enemyInfo.m_Material.color = Color.red;
        yield return m_OnDamageColor;
        enemyInfo.m_Material.color = new Color(1, 1, 1);
    }

    void HitEffect()
    {
        GameObject HitEffect = ObjectPoolManager.Instance.m_ObjectPoolList[0].Dequeue();
        // new Vector3 는 struct type 이고 스택에 생성되기 때문에 반복된 메모리릭 , 할당/해제 문제는 발생 X
        HitEffect.transform.position = new Vector3(m_Pos.x, enemyInfo.m_CapsuleCollider.bounds.size.y / 2, m_Pos.z);
        HitEffect.transform.rotation = transform.rotation;
        HitEffect.SetActive(true);
        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.75f, 0, HitEffect));
    }
    #endregion

    #region #몬스터 경사도 및 바닥 체크
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
        Vector3 boxSize = new Vector3(transform.lossyScale.x * 2, 0.4f, transform.lossyScale.z * 2);
        return Physics.CheckBox(m_GroundCheck.position, boxSize, Quaternion.identity, m_GroundLayer);
    }

    // isGournded Test Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(transform.lossyScale.x * 2, 0.4f, transform.lossyScale.z * 2);
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
