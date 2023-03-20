using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    [Header("현재 몬스터 갯수")]
    protected int m_MonsterCount = 0;
    protected int m_ReserveCount = 0; // 예약 생성 카운트

    [SerializeField]
    [Header("보스 생존 유무")]
    protected int m_BossCount = 0;
    protected int m_BossReserveCount = 0; // 예약 생성 카운트

    // 몬스터마다 유지 시켜야 되는 갯수가 다르기 때문에 세분화가 필요하다.
    // 1월 15일 현재 몬스터의 종류는 한가지 이므로 일단 진행
    [SerializeField]
    [Header("유지 시켜야 되는 몬스터 갯수")]
    protected int m_KeepMonsterCount = 0;
    [SerializeField]
    [Header("유지 시켜야 되는 보스 갯수")]
    protected int m_KeepBossCount = 1;

    [SerializeField]
    [Header("스폰 Position")]
    protected Vector3 m_SpawnPos;

    [SerializeField]
    protected float m_SpawnRadius = 6.5f;
    [SerializeField]
    protected float m_SpawnTime = 5.0f;
    [SerializeField]
    protected float m_BossSpawnTime = 30.0f;

    void Start() { Init(); }
    protected virtual void Init(){ }
    void Update(){ }
    /// <summary>
    /// 몬스터 타입 별로 스폰을 달리 해야 할 경우
    /// 1. 스폰 될 몬스터가 어떤 몬스터인지 판단 을 한다.
    /// 2. 그 다음 해당 몬스터에 해당하는 스폰 위치에서 스폰을 시켜준다.
    /// 3. 각 종류별 몬스터의 유지해야 하는 갯수가 다르기 때문에 따로 카운트를 해줘야한다.
    /// 
    /// ?? 그럼 각 종류별로 변수를 모두 따로 두어야 하나? > 예상으로는 스포닝풀 스크립트를 부모로 두고
    /// 각 종류별로 자식 클래스를 하나씩 만들어 오버라이딩으로 호출을 하면 되지 않을까 라는 생각
    /// 그러기에는 종류의 갯수 만큼 씬에 오브젝트가 생겨버린다. 
    ///  >> 하지만 하나의 오브젝트에 여러개의 파생 스포닝풀 스크립트를 넣어주면 되지 않을까?
    ///  
    ///  >> 코루틴을 오버라이드로 사용을 하는 방법
    ///  ------------------------------------
    /// protected virtual IEnumerator CoroutineA()
    /// {
    ///     // 코루틴 내용
    /// }
    /// 
    ///     protected override IEnumerator CoroutineA()
    /// {
    ///     yield return StartCoroutine(base.CoroutineA()); // 베이스 호출
    ///     // 추가될 코루틴 내용
    /// }
    ///  ------------------------------------
    /// </summary>
    protected virtual IEnumerator ResuerveSpawn(string MonsterType)
    {
        m_ReserveCount++;
        WaitForSeconds ranTime = new WaitForSeconds(Random.Range(3, m_SpawnTime));
        yield return ranTime;
        GameObject obj = GameManager.Instance.Spwan(Defines.WorldObject.Monster, $"Enemy/{MonsterType}");
        NavMeshAgent Nma = obj.GetOrAddComponet<NavMeshAgent>();

        Vector3 randPos;

        Vector3 randDir = Random.insideUnitSphere * Random.Range(0, m_SpawnRadius);
        randDir.y = 0;
        randPos = m_SpawnPos + randDir;

        while (true)
        {
            // 갈 수 있는 곳인지 체크
            NavMeshPath path = new NavMeshPath();
            if (Nma.CalculatePath(randPos, path))
                break;
        }

        Nma.enabled = false;
        obj.transform.position = randPos;

        Nma.enabled = true;
        m_ReserveCount--;
    }

    protected virtual IEnumerator BossResurveSpawn(string BossType)
    {
        m_BossReserveCount++;
        yield return new WaitForSeconds(m_BossSpawnTime);
        GameObject obj = GameManager.Instance.Spwan(Defines.WorldObject.Boss, $"Boss/{BossType}");
        NavMeshAgent Nma = obj.GetOrAddComponet<NavMeshAgent>();

        Nma.enabled = false;
        obj.transform.position = m_SpawnPos;

        Nma.enabled = true;
        m_BossReserveCount--;
    }
}

