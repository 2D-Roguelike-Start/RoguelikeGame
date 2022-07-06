using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value> 
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Stats> StatsDict { get; private set; } = new Dictionary<int, Stats>();

    public void init()
    {
        StatsDict = LoadJson<StatData, int, Stats>("statData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textasset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textasset.text);
    }
}
