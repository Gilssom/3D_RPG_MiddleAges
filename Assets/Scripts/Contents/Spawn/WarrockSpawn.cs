using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarrockSpawn : SpawningPool
{
    public void AddMonsterCount(int value) { m_MonsterCount += value; }
    public void SetKeepMonsterCount(int count) { m_KeepMonsterCount = count; }

    public void AddBossCount(int value) { m_BossCount += value; }
    public void SetKeepBossCount(int count) { m_KeepBossCount = count; }

    public void SetPosition(Vector3 SpawnPoint) { m_SpawnPos = SpawnPoint; }

    protected override void Init()
    {
        GameManager.Instance.OnWarrockSpawnEvent -= AddMonsterCount;
        GameManager.Instance.OnWarrockSpawnEvent += AddMonsterCount;

        GameManager.Instance.OnWarrockBossSpawnEvent -= AddBossCount;
        GameManager.Instance.OnWarrockBossSpawnEvent += AddBossCount;
    }

    void Update()
    {
        while (m_ReserveCount + m_MonsterCount < m_KeepMonsterCount)
        {
            StartCoroutine(ResuerveSpawn(Defines.MonsterType.Warrock.ToString()));
        }

        while (m_BossReserveCount + m_BossCount < m_KeepBossCount)
        {
            StartCoroutine(BossResurveSpawn(Defines.MonsterType.Warrock_Boss.ToString()));
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
