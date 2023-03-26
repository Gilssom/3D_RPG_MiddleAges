using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System 을 사용한다.
using UnityEngine.InputSystem.Interactions;
using CharacterController;
using DG.Tweening;
using System.Linq;

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

    //------- Npc Interaction ---------
    [Header("상호작용 가능한 Npc")]
    public NpcInteraction m_NearNpc;

    [Header("Npc와 상호작용 가능한 거리")]
    public float m_NpcCheckDis;

    [Header("현재 플레이어 위치")]
    public string m_CurrentAreaName;

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

        ItemEffectDataBase.Instance.m_Player = this;

        // Test
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 마을 위치 임시
        m_CurrentAreaName = "디오리카 평원";

        InvokeRepeating("AutoIncreaseHp", 0, 5);
        InvokeRepeating("FindNpc", 0, 0.5f);
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

        if (m_InputDirection.magnitude == 0)
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
            InventoryManager.Instance.TryOpenInventory();
        }
    }

    public void PlayerInfoUIOpen(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.TryOpenPlayerInfo();
        }
    }

    public void TestButtonClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.TryOpenEnforceSystem();
        }
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.CtrlInputBase(true);
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.CtrlInputBase(false);
        }
    }

    public void QuickSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int QuickNum = int.Parse(context.control.name);

            if(QuickNum < 5)
                SkillKeyMap.Instance.KeyPress(QuickNum);
            else if(QuickNum > 4)
                InventoryManager.Instance.m_QuickSlot.EatItem(QuickNum - 5);
        }
    }

    public void SkillBook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.TryOpenSkillSystem();
        }
    }

    public void QuestUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.TryOpenQuestSystem();
        }
    }

    public void AchievementUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryManager.Instance.TryOpenAchievementSystem();
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
                if(AttackState.isAttack 
                    || InventoryManager.m_InventoryActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_ShopActivated)
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
                if (DashState.m_IsDash || InventoryManager.m_InventoryActivated 
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
                {
                    return;
                }

                AttackState.m_AttackName = AttackState.AttackName.CHARGE;
                playerInfo.stateMachine.ChangeState(StateName.ATTACK);
            }

            else if(context.interaction is PressInteraction) // 일반 공격
            {
                AttackState.m_AttackName = AttackState.AttackName.ATTACK;

                if (DashState.m_IsDash 
                    || InventoryManager.m_InventoryActivated 
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
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
                if (DashState.m_IsDash && AttackState.isAttack 
                    || InventoryManager.m_InventoryActivated 
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
                {
                    return;
                }

                AttackState.m_AttackName = AttackState.AttackName.KICK;
                playerInfo.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }

    public void UltimateSkill()
    {
        if (DashState.m_IsDash && AttackState.isAttack
                    || InventoryManager.m_InventoryActivated
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
        {
            return;
        }

        AttackState.m_AttackName = AttackState.AttackName.ULTIMATE;
        playerInfo.stateMachine.ChangeState(StateName.ATTACK);
    }

    public void BladeSkill()
    {
        if (DashState.m_IsDash && AttackState.isAttack
                    || InventoryManager.m_InventoryActivated
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
        {
            return;
        }

        AttackState.m_AttackName = AttackState.AttackName.Blade;
        playerInfo.stateMachine.ChangeState(StateName.ATTACK);
    }

    public void LigthRefereeSkill()
    {
        if (DashState.m_IsDash && AttackState.isAttack
                    || InventoryManager.m_InventoryActivated
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
        {
            return;
        }

        AttackState.m_AttackName = AttackState.AttackName.Referee;
        playerInfo.stateMachine.ChangeState(StateName.ATTACK);
    }

    public void DevilSlashSkill()
    {
        if (DashState.m_IsDash && AttackState.isAttack
                    || InventoryManager.m_InventoryActivated
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
        {
            return;
        }

        AttackState.m_AttackName = AttackState.AttackName.Slash;
        playerInfo.stateMachine.ChangeState(StateName.ATTACK);
    }

    public void AngelSkill()
    {
        if (DashState.m_IsDash && AttackState.isAttack
                    || InventoryManager.m_InventoryActivated
                    || InventoryManager.m_ShopActivated
                    || InventoryManager.m_EnforceActivated
                    || InventoryManager.m_SkillActivated
                    || InventoryManager.m_QuestActivated
                    || InventoryManager.m_AchievementActivated)
        {
            return;
        }

        AttackState.m_AttackName = AttackState.AttackName.Angel;
        playerInfo.stateMachine.ChangeState(StateName.ATTACK);
    }

    public void WhispersSkill()
    {
        if (InventoryManager.m_InventoryActivated
                || InventoryManager.m_ShopActivated
                || InventoryManager.m_EnforceActivated
                || InventoryManager.m_SkillActivated
                || InventoryManager.m_QuestActivated
                || InventoryManager.m_AchievementActivated)
        {
            return;
        }

        Debug.Log("Whispers Skill On");
        SoundManager.Instance.Play("Effect/Whisper Skill");
        StartCoroutine(SetEffect(2, 9));
    }
    #endregion

    #region #플레이어 스킬 이펙트 관리
    public IEnumerator SetEffect(int skillNumber, float desTime)
    {
        WaitForSeconds time = new WaitForSeconds(desTime);

        BaseInfo.playerInfo.m_EffectList_Obj[skillNumber].SetActive(true);
        yield return time;
        BaseInfo.playerInfo.m_EffectList_Obj[skillNumber].SetActive(false);
    }

    public IEnumerator DesSkill(GameObject go, float desTime)
    {
        WaitForSeconds time = new WaitForSeconds(desTime);

        yield return time;
        ResourcesManager.Instance.Destroy(go);
    }
    #endregion

    #region #플레이어 무기 교체 시스템
    public void ChangeWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is PressInteraction)       // 무기 교체
            {
                if (DashState.m_IsDash && AttackState.isAttack)
                {
                    return;
                }

                playerInfo.m_WeaponManager.SetWeapon(null);
                playerInfo.m_Anim.SetTrigger("isChange");
                SoundManager.Instance.Play("Effect/Weapon Swap");
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

    #region #플레이어 트리거 이벤트
    private void OnTriggerEnter(Collider other)
    {
        #region #몬스터 피격
        if (other.tag == "EnemyHitBox")
            OnHitEvent(other.transform.parent.gameObject.GetComponent<EnemyInfo>().Attack);
        #endregion

        #region #보스 피격
        // 일반 공격 = 기본 공격력
        if (other.tag == "BossAttack")
            OnHitEvent(other.transform.parent.gameObject.GetComponent<BossInfo>().Attack);
        // 에픽 공격 = 기본 공격력 150%
        if (other.tag == "BossEpicAttack")
            OnHitEvent((int)(other.transform.parent.gameObject.GetComponent<BossInfo>().Attack * 1.5f));
        // 스페셜 공격 = 기본 공격력 200%
        if (other.tag == "BossSpecialAttack")
            OnHitEvent((int)(other.transform.parent.gameObject.GetComponent<BossInfo>().Attack * 2));
        #endregion

        #region #아이템 획득
        if (other.tag == "Item")
        {
            ItemPickUp(other.gameObject.GetOrAddComponet<ItemPickUp>());
        }
        #endregion

        #region #지역 이동
        if (other.tag == "AreaTrigger")
        {
            foreach (string areaName in other.GetComponent<AreaInfo>().m_AreaName)
            {
                if (areaName != m_CurrentAreaName)
                {
                    m_CurrentAreaName = areaName;
                    GameScene.Instance.UpdateAreaField();
                    break;
                }
            }
        }
        #endregion
    }
    #endregion

    #region #가까운 Npc 찾기
    private GameObject FindNearObjByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        m_NearNpc = neareastObject.GetComponent<NpcInteraction>();

        return neareastObject;
    }

    void FindNpc()
    {
        FindNearObjByTag("Npc");

        float Dis = Vector3.Distance(m_NearNpc.transform.position, transform.position);

        if (Dis >= m_NpcCheckDis)
        {
            m_NearNpc = null;
        }
    }
    #endregion

    #region #플레이어 상호작용
    public void NpcInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (m_NearNpc != null)
            {
                if (InventoryManager.Instance.m_Dialogue.DoDialogue)
                    InventoryManager.Instance.m_Dialogue.DialogueNext();
                else
                    m_NearNpc.InteractionNpc();
            }
            else
                return;
        }
    }
    #endregion

    #region #플레이어 피격 이벤트
    void OnHitEvent(int hitdamage)
    {
        if (playerInfo.Hp > 0)
        {
            int damage = UnityEngine.Random.Range(hitdamage - playerInfo.Defense, hitdamage + 1);
            Debug.Log($"적에게 입은 피해량 : {damage} !!");
            playerInfo.Hp -= damage;
            DamageText(damage, true);
            GameScene.Instance.m_BloodScreenUI.StartFadeIn(0.15f);
            SoundManager.Instance.Play("Effect/Player Hit");

            // 플레이어 사망
            if (playerInfo.Hp <= 0)
                GameManager.Instance.Despawn(gameObject);
                //playerInfo.stateMachine.ChangeState(StateName.DIE);
        }
    }

    void DamageText(int damage, bool player, bool critical = false)
    {
        UI_Damage damageObj = UIManager.Instance.MakeWorldSpaceUI<UI_Damage>(transform);
        damageObj.m_Damage = damage;
        damageObj.m_Critical = critical;
        damageObj.m_PlayerHit = player;
    }
    #endregion

    #region #플레이어 생명 재생 이벤트
    void AutoIncreaseHp()
    {
        if (playerInfo.Hp >= playerInfo.MaxHp)
            return;

        IncreaseHp(playerInfo.m_Plus_IncreaseHp);
    }
    #endregion

    #region #플레이어 아이템 획득 시스템
    void ItemPickUp(ItemPickUp item)
    {
        Debug.Log($"{item.m_Item.m_ItemName} ({item.m_Item.m_ItemClass}등급) 획득!");
        InventoryManager.Instance.AcquireItem(item.m_Item);
        ResourcesManager.Instance.Destroy(item.gameObject);
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

    #region #플레이어 Status 관리
    public void IncreaseHp(int _count)
    {
        UI_Heal healObj = UIManager.Instance.MakeWorldSpaceUI<UI_Heal>(transform);

        if (playerInfo.Hp + _count < playerInfo.MaxHp)
        {
            healObj.m_Heal = _count;
            playerInfo.Hp += _count;
        }
        else
        {
            healObj.m_Heal = playerInfo.MaxHp - playerInfo.Hp;
            playerInfo.Hp = playerInfo.MaxHp;
        }

        StartCoroutine(BuffEffect());
    }

    public void IncreaseDF(int _count)
    {
        playerInfo.Defense += _count;
    }

    public void IncreaseAt(int _count)
    {
        playerInfo.Attack += _count;
    }

    public void IncreaseMaxHp(int _count)
    {
        playerInfo.MaxHp += _count;
        IncreaseHp(_count);
    }

    IEnumerator BuffEffect()
    {
        playerInfo.m_EffectList[8].Play();
        yield return new WaitForSeconds(1);
        playerInfo.m_EffectList[8].Stop();
    }
    #endregion

    #region #플레이어 레벨업 UI
    public void LevelUpUI()
    {
        GameScene.Instance.m_LevelUpUI.LevelUpSetText();
    }
    #endregion
}