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

        //------ 아이템 드랍 정보 ------

        public float potionChance;
        public int potionCount;
        public int minFragments;
        public int maxFragments;
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

    #region #Enforce Data
    [Serializable]
    public class EnforceData
    {
        public int itemLevel;
        public int needMain;
        public int needSub;
        public int needFragments;
        public int needGold;
        public float success;
    }

    [Serializable]
    public class EnforceDataBase : ILoader<int, EnforceData>
    {
        public List<EnforceData> EnforceData = new List<EnforceData>();

        public Dictionary<int, EnforceData> MakeDict()
        {
            Dictionary<int, EnforceData> Dict = new Dictionary<int, EnforceData>();

            foreach (EnforceData stat in EnforceData)
                Dict.Add(stat.itemLevel, stat);

            return Dict;
        }
    }
    #endregion

    #region #Item Data Base
    [Serializable]
    public class ItemStat
    {
        public int itemId;
        public string itemName;
        public string itemPart_0;
        public string itemPart_1;
        public string itemPart_2;
        public int itemShame;
        public int itemAddPrice;
        public int itemSalePrice;
        public int itemAddLevel;
    }

    [Serializable]
    public class ItemStatData : ILoader<int, ItemStat>
    {
        public List<ItemStat> ItemDataBase = new List<ItemStat>();

        public Dictionary<int, ItemStat> MakeDict()
        {
            Dictionary<int, ItemStat> Dict = new Dictionary<int, ItemStat>();

            foreach (ItemStat stat in ItemDataBase)
                Dict.Add(stat.itemId, stat);

            return Dict;
        }
    }
    #endregion

    #region #Enforce Stat Data Base
    [Serializable]
    public class EnforceStatData
    {
        public int itemLevel;
        public int minMaxHp;
        public int maxMaxHp;
        public int minDefense;
        public int maxDefense;
        public int criChance;
        public float criDamage;
        public int attack;
    }

    [Serializable]
    public class EnforceStatDataBase : ILoader<int, EnforceStatData>
    {
        public List<EnforceStatData> EnforceStatData = new List<EnforceStatData>();

        public Dictionary<int, EnforceStatData> MakeDict()
        {
            Dictionary<int, EnforceStatData> Dict = new Dictionary<int, EnforceStatData>();

            foreach (EnforceStatData stat in EnforceStatData)
                Dict.Add(stat.itemLevel, stat);

            return Dict;
        }
    }
    #endregion
}