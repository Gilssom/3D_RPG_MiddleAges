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
    private GameObject m_Target;

    public override void Init()
    {
        enemyInfo = GetComponent<EnemyInfo>();
        m_Chracter = GetComponent<Transform>();
        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground");
        enemyInfo.ReadyAttack = true;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            UIManager.Instance.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    #region #�⺻ �̵� �ý���
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
    }
    #endregion


    //------ AI �ý��� ---------
    protected override void UpdateIdle()
    {
        // �÷��̾� üũ
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;

        if (distance <= enemyInfo.AttackRange)
        {
            //EnemyAttackState.isAttack == false && 
            if (enemyInfo.ReadyAttack)
            {
                Debug.LogError("Check");
                enemyInfo.CheckPlayer = false;
                enemyInfo.ReadyAttack = false;
                // Attack �� ����
                enemyInfo.stateMachine.ChangeState(StateName.ATTACK);
                return;
            }
            else
            {
                enemyInfo.stateMachine.ChangeState(StateName.IDLE);
            }
        }
        else if (distance <= enemyInfo.ScanRange)
        {
            if (!enemyInfo.CheckPlayer)
            {
                enemyInfo.CheckPlayer = true;
                m_Target = player;
                // Move �� ����
                enemyInfo.stateMachine.ChangeState(StateName.MOVE);
                return;
            }
        }
        else
        {
            if (enemyInfo.CheckPlayer)
            {
                enemyInfo.CheckPlayer = false;
                m_Target = null;
                // Idle �� ����
                enemyInfo.stateMachine.ChangeState(StateName.IDLE);
                return;
            }
        }
    }

    #region #���� ���� �ý���
    public IEnumerator AttackCoroutine()
    {
        Debug.LogWarning("Start Coroutine");
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

    #region #���� ����
    public void LookAt()
    {
        m_Chracter.LookAt(m_Target.transform.position);
    }
    #endregion

    #region #���� �ǰ� �̺�Ʈ
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitBox")
            OnHitEvent(other.transform.parent.gameObject.GetComponent<BaseInfo>());
    }

    void OnHitEvent(BaseInfo player)
    {
        if(BaseInfo.Instance.Hp > 0)
        {
            BaseInfo enemyStat = gameObject.GetComponent<BaseInfo>();
            int damage = Random.Range(player.Attack - enemyStat.Defense, player.Attack + 1);
            Debug.Log($"������ ���� ���ط� : {damage} !!");
            enemyStat.Hp -= damage;

            if(enemyStat.Hp <= 0)
                enemyInfo.stateMachine.ChangeState(StateName.DIE);
        }
    }
    #endregion

    #region #���� ��絵 �� �ٴ� üũ
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
