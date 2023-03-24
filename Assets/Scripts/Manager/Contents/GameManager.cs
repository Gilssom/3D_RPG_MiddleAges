using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletomManager<GameManager>
{
    protected GameManager() { }

    GameObject m_Player;
    HashSet<GameObject> m_Monster = new HashSet<GameObject>();
    HashSet<GameObject> m_Boss = new HashSet<GameObject>();

    #region #Monster & Boss Spawn Event
    public Action<int> OnMutantSpawnEvent;
    public Action<int> OnWarrockSpawnEvent;
    public Action<int> OnMawSpawnEvent;
    public Action<int> OnMutantBossSpawnEvent;
    public Action<int> OnWarrockBossSpawnEvent;
    public Action<int> OnMawBossSpawnEvent;
    #endregion

    public GameObject Spwan(Defines.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = ResourcesManager.Instance.Instantiate(path, parent);

        switch (type)
        {
            case Defines.WorldObject.Player:
                m_Player = go;
                break;
            case Defines.WorldObject.Monster:
                m_Monster.Add(go);
                switch (go.GetComponent<EnemyInfo>().Type)
                {
                    case Defines.MonsterType.Mutant:
                        if (OnMutantSpawnEvent != null)
                            OnMutantSpawnEvent.Invoke(1);
                        break;
                    case Defines.MonsterType.Warrock:
                        if (OnWarrockSpawnEvent != null)
                            OnWarrockSpawnEvent.Invoke(1);
                        break;
                    case Defines.MonsterType.Maw:
                        if (OnMawSpawnEvent != null)
                            OnMawSpawnEvent.Invoke(1);
                        break;
                }
                break;
            case Defines.WorldObject.Boss:
                m_Boss.Add(go);
                switch (go.GetComponent<BossInfo>().Type)
                {
                    case Defines.MonsterType.Mutant_Boss:
                        if (OnMutantBossSpawnEvent != null)
                            OnMutantBossSpawnEvent.Invoke(1);
                        break;
                    case Defines.MonsterType.Warrock_Boss:
                        if (OnWarrockBossSpawnEvent != null)
                            OnWarrockBossSpawnEvent.Invoke(1);
                        break;
                    case Defines.MonsterType.Maw_Boss:
                        if (OnMawBossSpawnEvent != null)
                            OnMawBossSpawnEvent.Invoke(1);
                        break;
                }
                break;
        }

        return go;
    }

    // 해당 오브젝트가 Player 인지 Enemy 인지 무슨 타입인지
    public Defines.WorldObject GetWorldObjectType(GameObject go)
    {
        Base bs = go.GetComponent<Base>();

        if (bs == null)
            return Defines.WorldObject.Unknown;

        return bs.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Defines.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Defines.WorldObject.Player:
                {
                    if (m_Player == go)
                        m_Player = null;
                }
                break;
            case Defines.WorldObject.Monster:
                {
                    if (m_Monster.Contains(go))
                    {
                        m_Monster.Remove(go);
                        switch (go.GetComponent<EnemyInfo>().Type)
                        {
                            case Defines.MonsterType.Mutant:
                                if (OnMutantSpawnEvent != null)
                                    OnMutantSpawnEvent.Invoke(-1);
                                break;
                            case Defines.MonsterType.Warrock:
                                if (OnWarrockSpawnEvent != null)
                                    OnWarrockSpawnEvent.Invoke(-1);
                                break;
                            case Defines.MonsterType.Maw:
                                if (OnMawSpawnEvent != null)
                                    OnMawSpawnEvent.Invoke(-1);
                                break;
                        }
                        break;
                    }                    
                }
                break;
            case Defines.WorldObject.Boss:
                {
                    if (m_Boss.Contains(go))
                    {
                        m_Boss.Remove(go);
                        switch (go.GetComponent<BossInfo>().Type)
                        {
                            case Defines.MonsterType.Mutant_Boss:
                                if (OnMutantBossSpawnEvent != null)
                                    OnMutantBossSpawnEvent.Invoke(-1);
                                break;
                            case Defines.MonsterType.Warrock_Boss:
                                if (OnWarrockBossSpawnEvent != null)
                                    OnWarrockBossSpawnEvent.Invoke(-1);
                                break;
                            case Defines.MonsterType.Maw_Boss:
                                if (OnMawBossSpawnEvent != null)
                                    OnMawBossSpawnEvent.Invoke(-1);
                                break;
                        }
                        break;
                    }
                }
                break;
        }

        ResourcesManager.Instance.Destroy(go);
    }

    // Scene 이동 시 Clear 할 수 있는 것들은 모두 Clera
    public static void Clear()
    {
        SoundManager.Instance.Clear();
        UIManager.Instance.Clear();
    }
}
