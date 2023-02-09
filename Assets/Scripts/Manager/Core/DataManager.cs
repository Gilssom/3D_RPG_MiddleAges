using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
    public Dictionary<int, Data.MonsterStat> MonsterStatDict { get; private set; } = new Dictionary<int, Data.MonsterStat>();
    public Dictionary<int, Data.ItemData> ItemDict { get; private set; } = new Dictionary<int, Data.ItemData>();

    public void Init(string DataName)
    {
        switch (DataName)
        {
            case "StatData":
                StatDict = LoadJson<Data.StatData, int, Data.Stat>(DataName).MakeDict();
                break;
            case "MonsterStatData":
                MonsterStatDict = LoadJson<Data.MonsterStatData, int, Data.MonsterStat>(DataName).MakeDict();
                break;
            case "ItemDataBase":
                ItemDict = LoadJson<Data.ItemDataBase, int, Data.ItemData>(DataName).MakeDict();
                break;
            default:
                break;
        }
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/Json/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
