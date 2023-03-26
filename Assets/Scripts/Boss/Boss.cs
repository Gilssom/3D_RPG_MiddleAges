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

    [Header("최초 스폰 위치")]
    public Vector3 m_SpawnTransform;
    [SerializeField]
    private bool isReturn;

    [Header("퀘스트 진행 관련 Event")]
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

    #region #기본 이동 시스템
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

    #region #보스 기본 추적 - 공격 - 휴먼 상태 시스템
    //------ Boss 시스템 ---------
    /// <summary>
    /// 1. 플레이어를 감지하게 되면 BossInfo 의 CheckPlayer 를 true 로 변경
    /// >> Target 이 Player 로 잡히게 된다.
    /// => State 를 Move 로 전환
    /// 
    /// 2. 플레이어가 공격 범위 내에 들어오게 되면 BossInfo 의 ReadyAttack 을 true 로 변경
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
        if (bossInfo.RunAway == false)
        {
            // 플레이어 체크
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null || isReturn)
            {
                return;
            }

            SetHpBar();
            m_Pos = transform.position;
            float distance = (player.transform.position - transform.position).magnitude;

            // 1번 공격 전환
            if (distance <= bossInfo.AttackRange)
            {
                //EnemyAttackState.isAttack == false && 
                if (bossInfo.CheckPlayer && bossInfo.ReadyAttack)
                {
                    // Attack 로 변경
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
            // 2번 추적 전환
            else if (distance <= bossInfo.ScanRange)
            {
                m_Target = player;
                // Move 로 변경
                bossInfo.stateMachine.ChangeState(StateName.RUN);
                return;
            }
            // 3번 기본 자세 전환
            else
            {
                bossInfo.CheckPlayer = false;
                bossInfo.ReadyAttack = false;
                m_Target = null;
                // Idle 로 변경
                bossInfo.stateMachine.ChangeState(StateName.IDLE);
                return;
            }
        }
    }

    // 보스가 전투 지역을 벗어 났을때
    void ReturnPosition()
    {
        isReturn = true;
        // 몬스터 위치 초기화
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

    #region #보스 회피 시스템
    /// <summary>
    /// 보스 회피 시스템
    /// 1. 플레이어에게 피격시 확률적으로 회피 지역으로 이동 / 체력이 일정 닳을 때 회피 지역으로 이동
    /// 2. 회피지역은 Spawning.cs 의 insideUnitySphere 이용
    /// 3. 해당 지역을 Target 으로 두고 이동 시킴 / 변수를 둠으로써 PlayerCheck 메소드 비활성화 필요
    /// 4. 해당 지역에 도착했을 시 다시 PlayerCheck 실행
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

    #region #몬스터 공격 시스템
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

    #region #몬스터 방향
    public void LookAt(Vector3 target)
    {
        if (m_Target == null)
        {
            return;
        }

        m_Chracter.LookAt(target);
    }
    #endregion

    #region #몬스터 피격 이벤트
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

            Debug.Log($"적에게 가한 피해량 : {damage} !!");
            DamageText(damage, isCritical);
            bossInfo.Hp -= damage;
            StartCoroutine(OnDamageEvent());

            // 랜덤 회피
            int ranRunAway = Random.Range(0, 10);
            if (ranRunAway < 2)
                MoveRanTarget();

            // 몬스터 사망
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
        // new Vector3 는 struct type 이고 스택에 생성되기 때문에 반복된 메모리릭 , 할당/해제 문제는 발생 X
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
