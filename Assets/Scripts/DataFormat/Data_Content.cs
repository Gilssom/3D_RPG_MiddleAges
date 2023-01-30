using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region #Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int criticalchance;
        public float criticaldamage;
        public int totalExp;
        public int defense;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> Dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
                Dict.Add(stat.level, stat);

            return Dict;
        }
    }
    #endregion

    #region #Monster Stat
    [Serializable]
    public class MonsterStat
    {
        public int id;
        public int level;
        public int maxHp;
        public int attack;
        public int defense;
        public int dropExp;
        public int dropGold;
        public string name;
    }

    [Serializable]
    public class MonsterStatData : ILoader<int, MonsterStat>
    {
        public List<MonsterStat> monster = new List<MonsterStat>();

        public Dictionary<int, MonsterStat> MakeDict()
        {
            Dictionary<int, MonsterStat> Dict = new Dictionary<int, MonsterStat>();

            foreach (MonsterStat stat in monster)
                Dict.Add(stat.id, stat);

            return Dict;
        }
    }
    #endregion
}