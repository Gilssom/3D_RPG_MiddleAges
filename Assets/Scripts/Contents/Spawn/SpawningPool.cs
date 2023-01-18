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

    // ���͸��� ���� ���Ѿ� �Ǵ� ������ �ٸ��� ������ ����ȭ�� �ʿ��ϴ�.
    // 1�� 15�� ���� ������ ������ �Ѱ��� �̹Ƿ� �ϴ� ����
    [SerializeField]
    [Header("���� ���Ѿ� �Ǵ� ���� ����")]
    protected int m_KeepMonsterCount = 0;

    [SerializeField]
    [Header("���� Position")]
    protected Vector3 m_SpawnPos;

    [SerializeField]
    protected float m_SpawnRadius = 7.5f;
    [SerializeField]
    protected float m_SpawnTime = 5.0f;

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
        yield return new WaitForSeconds(Random.Range(2, m_SpawnTime));
        GameObject obj = GameManager.Instance.Spwan(Defines.WorldObject.Monster, $"Enemy/{MonsterType}");
        NavMeshAgent Nma = obj.GetOrAddComponet<NavMeshAgent>();

        Vector3 randPos;

        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, m_SpawnRadius);
            randDir.y = 0;
            randPos = m_SpawnPos + randDir;

            // �� �� �ִ� ������ üũ
            NavMeshPath path = new NavMeshPath();
            if (Nma.CalculatePath(randPos, path))
                break;
        }

        obj.transform.position = randPos;
        m_ReserveCount--;
    }
}

