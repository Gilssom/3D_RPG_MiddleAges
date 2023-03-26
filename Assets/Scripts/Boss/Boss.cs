using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterController;

[RequireComponent(typeof(BossInfo))]
public class Boss : Base
{
    public BossInfo bossInfo { get; private set; }
    [SerializeField]
    public GameObject m_Target { get; private set; }

    private Transform m_Chracter;
    private Vector3 m_Pos;
    [SerializeField]
    private Vector3 m_RunAwayPos;

    private WaitForSeconds m_OnDamageColor;

    private UI_Boss_HPBar m_HpBarUI;

    [Header("���� ���� ��ġ")]
    public Vector3 m_SpawnTransform;
    [SerializeField]
    private bool isReturn;

    [Header("����Ʈ ���� ���� Event")]
    public UnityEngine.Events.UnityEvent onDead;

    public override void Init()
    {
        WorldObjectType = Defines.WorldObject.Boss;
        bossInfo = GetComponent<BossInfo>();
        m_Chracter = GetComponent<Transform>();
        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground");
        m_OnDamageColor = new WaitForSeconds(0.2f);

        m_SpawnTransform = this.transform.position;
    }

    #region #�⺻ �̵� �ý���
    public void Move(Vector3 target)
    {
        NavMeshAgent navmesh = gameObject.GetOrAddComponet<NavMeshAgent>();
        MoveStop(false);
        navmesh.SetDestination(target);

        if (isReturn)
        {
            navmesh.speed = 2;

            if (navmesh.remainingDistance < 0.5f)
            {
                isReturn = false;
                MoveStop(true);
                return;
            }
            return;
        }

        navmesh.speed = bossInfo.MoveSpeed;

        LookAt(target);
    }

    public void MoveStop(bool isStop)
    {
        NavMeshAgent navmesh = gameObject.GetOrAddComponet<NavMeshAgent>();
        navmesh.isStopped = isStop;
        navmesh.speed = bossInfo.MoveSpeed;
        bossInfo.m_Rigid.velocity = Vector3.zero;
        bossInfo.m_Rigid.angularVelocity = Vector3.zero;
    }
    #endregion

    #region #���� �⺻ ���� - ���� - �޸� ���� �ý���
    //------ Boss �ý��� ---------
    /// <summary>
    /// 1. �÷��̾ �����ϰ� �Ǹ� BossInfo �� CheckPlayer �� true �� ����
    /// >> Target �� Player �� ������ �ȴ�.
    /// => State �� Move �� ��ȯ
    /// 
    /// 2. �÷��̾ ���� ���� ���� ������ �Ǹ� BossInfo �� ReadyAttack �� true �� ����
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
        if (bossInfo.RunAway == false)
        {
            // �÷��̾� üũ
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null || isReturn)
            {
                return;
            }

            SetHpBar();
            m_Pos = transform.position;
            float distance = (player.transform.position - transform.position).magnitude;

            // 1�� ���� ��ȯ
            if (distance <= bossInfo.AttackRange)
            {
                //EnemyAttackState.isAttack == false && 
                if (bossInfo.CheckPlayer && bossInfo.ReadyAttack)
                {
                    // Attack �� ����
                    bossInfo.stateMachine.ChangeState(StateName.ATTACK);
                    return;
                }
            }
            else if (distance <= bossInfo.CombotRange)
            {
                bossInfo.ReadyAttack = true;
                bossInfo.CheckPlayer = true;
                m_Target = player;
                bossInfo.stateMachine.ChangeState(StateName.MOVE);
            }
            // 2�� ���� ��ȯ
            else if (distance <= bossInfo.ScanRange)
            {
                m_Target = player;
                // Move �� ����
                bossInfo.stateMachine.ChangeState(StateName.RUN);
                return;
            }
            // 3�� �⺻ �ڼ� ��ȯ
            else
            {
                bossInfo.CheckPlayer = false;
                bossInfo.ReadyAttack = false;
                m_Target = null;
                // Idle �� ����
                bossInfo.stateMachine.ChangeState(StateName.IDLE);
                return;
            }
        }
    }

    // ������ ���� ������ ���� ������
    void ReturnPosition()
    {
        isReturn = true;
        // ���� ��ġ �ʱ�ȭ
        bossInfo.CheckPlayer = false;
        bossInfo.ReadyAttack = false;
        m_Target = null;
        UIManager.Instance.ClosePopupUI(m_HpBarUI);
        UIManager.Instance.isBossHPOpen = false;

        bossInfo.stateMachine.ChangeState(StateName.MOVE);
    }

    void SetHpBar()
    {
        if (UIManager.Instance.isBossHPOpen || !m_Target)
            return;

        SoundManager.Instance.Play("Effect/Boss Start Fight");
        UIManager.Instance.isBossHPOpen = true;
        m_HpBarUI = UIManager.Instance.ShowPopupUI<UI_Boss_HPBar>();
        m_HpBarUI.m_BossInfo = bossInfo;     
    }
    #endregion

    #region #���� ȸ�� �ý���
    /// <summary>
    /// ���� ȸ�� �ý���
    /// 1. �÷��̾�� �ǰݽ� Ȯ�������� ȸ�� �������� �̵� / ü���� ���� ���� �� ȸ�� �������� �̵�
    /// 2. ȸ�������� Spawning.cs �� insideUnitySphere �̿�
    /// 3. �ش� ������ Target ���� �ΰ� �̵� ��Ŵ / ������ �����ν� PlayerCheck �޼ҵ� ��Ȱ��ȭ �ʿ�
    /// 4. �ش� ������ �������� �� �ٽ� PlayerCheck ����
    /// </summary>
    public void MoveRanTarget()
    {
        bossInfo.RunAway = true;
        NavMeshAgent navmesh = gameObject.GetOrAddComponet<NavMeshAgent>();
        Vector3 randPos;

        Vector3 randDir = Random.insideUnitSphere * Random.Range(3, 7);
        randDir.y = 0;
        randPos = transform.position + randDir;

        m_RunAwayPos = randPos;
        bossInfo.stateMachine.ChangeState(StateName.RUN);
        Move(m_RunAwayPos);
    }

    public void CheckRanTargetEnd()
    {
        NavMeshAgent navmesh = gameObject.GetOrAddComponet<NavMeshAgent>();
        if (navmesh.remainingDistance <= 0.2f)
        {
            m_RunAwayPos = Vector3.zero;
            bossInfo.RunAway = false;
            bossInfo.stateMachine.ChangeState(StateName.IDLE);
        }
    }
    #endregion

    #region #���� ���� �ý���
    public IEnumerator AttackCoroutine()
    {
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;
            if (curTime >= bossInfo.AttackCoolTime)
                break;
            yield return null;
        }

        bossInfo.ReadyAttack = true;
    }
    #endregion

    #region #���� ����
    public void LookAt(Vector3 target)
    {
        if (m_Target == null)
        {
            return;
        }

        m_Chracter.LookAt(target);
    }
    #endregion

    #region #���� �ǰ� �̺�Ʈ
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitBox")
            OnHitEvent(BaseInfo.playerInfo);

        if (other.tag == "OutOfArea")
            ReturnPosition();
    }

    void OnHitEvent(PlayerInfo player)
    {
        if (bossInfo.Hp > 0 && !bossInfo.Cover)
        {
            bool isCritical = false;
            int critical = Random.Range(0, 100);
            int damage = Random.Range(player.Attack - bossInfo.Defense, player.Attack + 1);

            if (critical < player.CriticalChance)
            {
                damage = Mathf.RoundToInt(damage * (player.CriticalDamage / 100));
                isCritical = true;
            }

            Debug.Log($"������ ���� ���ط� : {damage} !!");
            DamageText(damage, isCritical);
            bossInfo.Hp -= damage;
            StartCoroutine(OnDamageEvent());

            // ���� ȸ��
            int ranRunAway = Random.Range(0, 10);
            if (ranRunAway < 2)
                MoveRanTarget();

            // ���� ���
            if (bossInfo.Hp <= 0)
            {
                player.Exp += bossInfo.DropExp;
                player.Gold += bossInfo.DropGold;
                player.Fragments += Random.Range(bossInfo.MinFargCount, bossInfo.MaxFragCount + 1);
                bossInfo.stateMachine.ChangeState(StateName.DIE);
            }
        }
    }

    IEnumerator OnDamageEvent()
    {
        ShakeCamera.Instance.CameraShake(3f, 0.2f);
        HitEffect();
        bossInfo.m_Material.color = Color.red;
        yield return m_OnDamageColor;
        bossInfo.m_Material.color = new Color(1, 1, 1);
    }

    void HitEffect()
    {
        GameObject HitEffect = ObjectPoolManager.Instance.m_ObjectPoolList[bossInfo.ID - 2000].Dequeue();
        // new Vector3 �� struct type �̰� ���ÿ� �����Ǳ� ������ �ݺ��� �޸𸮸� , �Ҵ�/���� ������ �߻� X
        HitEffect.transform.position = new Vector3(m_Pos.x, bossInfo.m_CapsuleCollider.bounds.size.y / 2, m_Pos.z);
        HitEffect.transform.rotation = transform.rotation;
        HitEffect.SetActive(true);
        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.75f, bossInfo.ID - 2000, HitEffect));
    }

    void DamageText(int damage, bool critical)
    {
        UI_Damage damageObj = UIManager.Instance.MakeWorldSpaceUI<UI_Damage>(transform);
        damageObj.m_Damage = damage;
        damageObj.m_Critical = critical;
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
