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
    public Dictionary<int, Data.Stat>            StatDict        { get; private set; } = new Dictionary<int, Data.Stat>();
    public Dictionary<int, Data.MonsterStat>     MonsterStatDict { get; private set; } = new Dictionary<int, Data.MonsterStat>();
    public Dictionary<int, Data.ItemStat>        ItemDict        { get; private set; } = new Dictionary<int, Data.ItemStat>();
    public Dictionary<int, Data.EnforceData>     EnforceDict     { get; private set; } = new Dictionary<int, Data.EnforceData>();
    public Dictionary<int, Data.EnforceStatData> EnforceStatDict { get; private set; } = new Dictionary<int, Data.EnforceStatData>();

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
            case "ItemBaseData":
                ItemDict = LoadJson<Data.ItemStatData, int, Data.ItemStat>(DataName).MakeDict();
                break;
            case "EnforceData":
                EnforceDict = LoadJson<Data.EnforceDataBase, int, Data.EnforceData>(DataName).MakeDict();
                break;
            case "EnforceStatData":
                EnforceStatDict = LoadJson<Data.EnforceStatDataBase, int, Data.EnforceStatData>(DataName).MakeDict();
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