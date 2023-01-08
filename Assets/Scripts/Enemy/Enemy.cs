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
        enemyInfo.m_Rigid.velocity = Vector3.zero;
        enemyInfo.m_Rigid.angularVelocity = Vector3.zero;
    }
    #endregion


    //------ AI �ý��� ---------
    /// <summary>
    /// 1. �÷��̾ �����ϰ� �Ǹ� EnemyInfo �� CheckPlayer �� true �� ����
    /// >> Target �� Player �� ������ �ȴ�.
    /// => State �� Move �� ��ȯ
    /// 
    /// 2. �÷��̾ ���� ���� ���� ������ �Ǹ� EnemyInfo �� ReadyAttack �� true �� ����
    /// >> Target �� �״�� Player .
    /// => State �� Attack ���� ��ȯ
    /// 
    /// 3. �÷��̾ ���� ���� ���� ����� ReadyAttack �� false,
    ///    => State �� Move �� ��ȯ >> 1�� ���·� ��ȯ��
    ///    �÷��̾ ���� ���� ���� ����� CheckPlayer �� false.
    ///    => State �� Idle �� ��ȯ
    ///    
    /// ������ �ϰ� �Ǹ� ReadyAttack �� ���� 4�ʰ��� �ð� ���� �ٽ� true �� �Ͽ� Attack
    /// >> Attack ���� ReadyAttack �� false , 
    ///    false �ð� ���� Idle �� ��ȯ >> 3�� ���·� ��ȯ
    /// 
    /// </summary>
    public void PlayerCheck()
    {
        // �÷��̾� üũ
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        m_Pos = transform.position;
        float distance = (player.transform.position - transform.position).magnitude;

        // 1�� ���� ��ȯ
        if (distance <= enemyInfo.AttackRange)
        {
            //EnemyAttackState.isAttack == false && 
            if (enemyInfo.CheckPlayer && enemyInfo.ReadyAttack)
            {
                Debug.LogWarning("���� ���·� ��ȯ");
                // Attack �� ����
                enemyInfo.stateMachine.ChangeState(StateName.ATTACK);
                return;
            }
        }
        // 2�� ���� ��ȯ
        else if (distance <= enemyInfo.ScanRange)
        {
            Debug.LogWarning("���� ���·� ��ȯ");
            enemyInfo.CheckPlayer = true;
            m_Target = player;
            // Move �� ����
            enemyInfo.stateMachine.ChangeState(StateName.MOVE);
            return;            
        }
        // 3�� �⺻ �ڼ� ��ȯ
        else
        {
            Debug.LogWarning("�⺻ ���·� ��ȯ");
            enemyInfo.CheckPlayer = false;
            enemyInfo.ReadyAttack = false;
            m_Target = null;
            // Idle �� ����
            enemyInfo.stateMachine.ChangeState(StateName.IDLE);
            return;
        }
    }

    #region #���� ���� �ý���
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

    #region #���� ����
    public void LookAt()
    {
        if(m_Target == null)
        {
            return;
        }

        m_Chracter.LookAt(m_Target.transform.position);
    }
    #endregion

    #region #���� �ǰ� �̺�Ʈ
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
            Debug.Log($"������ ���� ���ط� : {damage} !!");
            enemyInfo.Hp -= damage;
            StartCoroutine(OnDamageEvent());

            // ���� ���
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
        // new Vector3 �� struct type �̰� ���ÿ� �����Ǳ� ������ �ݺ��� �޸𸮸� , �Ҵ�/���� ������ �߻� X
        HitEffect.transform.position = new Vector3(m_Pos.x, enemyInfo.m_CapsuleCollider.bounds.size.y / 2, m_Pos.z);
        HitEffect.transform.rotation = transform.rotation;
        HitEffect.SetActive(true);
        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.75f, 0, HitEffect));
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
