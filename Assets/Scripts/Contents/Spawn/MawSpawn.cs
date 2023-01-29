using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MawSpawn : SpawningPool
{
    public void AddMonsterCount(int value) { m_MonsterCount += value; }
    public void SetKeepMonsterCount(int count) { m_KeepMonsterCount = count; }

    public void AddBossCount(int value) { m_BossCount += value; }
    public void SetKeepBossCount(int count) { m_KeepBossCount = count; }

    public void SetPosition(Vector3 SpawnPoint) { m_SpawnPos = SpawnPoint; }

    protected override void Init()
    {
        GameManager.Instance.OnMawSpawnEvent -= AddMonsterCount;
        GameManager.Instance.OnMawSpawnEvent += AddMonsterCount;

        GameManager.Instance.OnMawBossSpawnEvent -= AddBossCount;
        GameManager.Instance.OnMawBossSpawnEvent += AddBossCount;
    }

    void Update()
    {
        while (m_ReserveCount + m_MonsterCount < m_KeepMonsterCount)
        {
            StartCoroutine(ResuerveSpawn(Defines.MonsterType.Maw.ToString()));
        }

        while (m_BossReserveCount + m_BossCount < m_KeepBossCount)
        {
            StartCoroutine(BossResurveSpawn(Defines.MonsterType.Maw_Boss.ToString()));
        }
    }

    protected override IEnumerator ResuerveSpawn(string MonsterType)
    {
        yield return StartCoroutine(base.ResuerveSpawn(MonsterType));
    }

    protected override IEnumerator BossResurveSpawn(string BossType)
    {
        yield return StartCoroutine(base.BossResurveSpawn(BossType));
    }
}
