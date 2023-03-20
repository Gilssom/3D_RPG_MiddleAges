using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    [Header("���� ���� ����")]
    protected int m_MonsterCount = 0;
    protected int m_ReserveCount = 0; // ���� ���� ī��Ʈ

    [SerializeField]
    [Header("���� ���� ����")]
    protected int m_BossCount = 0;
    protected int m_BossReserveCount = 0; // ���� ���� ī��Ʈ

    // ���͸��� ���� ���Ѿ� �Ǵ� ������ �ٸ��� ������ ����ȭ�� �ʿ��ϴ�.
    // 1�� 15�� ���� ������ ������ �Ѱ��� �̹Ƿ� �ϴ� ����
    [SerializeField]
    [Header("���� ���Ѿ� �Ǵ� ���� ����")]
    protected int m_KeepMonsterCount = 0;
    [SerializeField]
    [Header("���� ���Ѿ� �Ǵ� ���� ����")]
    protected int m_KeepBossCount = 1;

    [SerializeField]
    [Header("���� Position")]
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
    /// ���� Ÿ�� ���� ������ �޸� �ؾ� �� ���
    /// 1. ���� �� ���Ͱ� � �������� �Ǵ� �� �Ѵ�.
    /// 2. �� ���� �ش� ���Ϳ� �ش��ϴ� ���� ��ġ���� ������ �����ش�.
    /// 3. �� ������ ������ �����ؾ� �ϴ� ������ �ٸ��� ������ ���� ī��Ʈ�� ������Ѵ�.
    /// 
    /// ?? �׷� �� �������� ������ ��� ���� �ξ�� �ϳ�? > �������δ� ������Ǯ ��ũ��Ʈ�� �θ�� �ΰ�
    /// �� �������� �ڽ� Ŭ������ �ϳ��� ����� �������̵����� ȣ���� �ϸ� ���� ������ ��� ����
    /// �׷��⿡�� ������ ���� ��ŭ ���� ������Ʈ�� ���ܹ�����. 
    ///  >> ������ �ϳ��� ������Ʈ�� �������� �Ļ� ������Ǯ ��ũ��Ʈ�� �־��ָ� ���� ������?
    ///  
    ///  >> �ڷ�ƾ�� �������̵�� ����� �ϴ� ���
    ///  ------------------------------------
    /// protected virtual IEnumerator CoroutineA()
    /// {
    ///     // �ڷ�ƾ ����
    /// }
    /// 
    ///     protected override IEnumerator CoroutineA()
    /// {
    ///     yield return StartCoroutine(base.CoroutineA()); // ���̽� ȣ��
    ///     // �߰��� �ڷ�ƾ ����
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
            // �� �� �ִ� ������ üũ
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

