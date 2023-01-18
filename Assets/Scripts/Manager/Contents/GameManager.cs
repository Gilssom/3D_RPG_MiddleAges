using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletomManager<GameManager>
{
    protected GameManager() { }

    GameObject m_Player;
    HashSet<GameObject> m_Monster = new HashSet<GameObject>();

    #region #Monster Spawn Event
    public Action<int> OnMutantSpawnEvent;
    public Action<int> OnWarrockSpawnEvent;
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
                        }
                        break;
                    }                    
                }
                break;
        }

        ResourcesManager.Instance.Destroy(go);
    }
}
