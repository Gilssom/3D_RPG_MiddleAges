using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantSpawn : SpawningPool
{
    public void AddMonsterCount(int value) { m_MonsterCount += value; }
    public void SetKeepMonsterCount(int count) { m_KeepMonsterCount = count; }
    public void SetPosition(Vector3 SpawnPoint) { m_SpawnPos = SpawnPoint; }

    protected override void Init()
    {
        GameManager.Instance.OnMutantSpawnEvent -= AddMonsterCount;
        GameManager.Instance.OnMutantSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (m_ReserveCount + m_MonsterCount < m_KeepMonsterCount)
        {
            StartCoroutine(ResuerveSpawn(Defines.MonsterType.Mutant.ToString()));
        }
    }

    protected override IEnumerator ResuerveSpawn(string MonsterType)
    {
        yield return StartCoroutine(base.ResuerveSpawn(MonsterType));
    }
}
