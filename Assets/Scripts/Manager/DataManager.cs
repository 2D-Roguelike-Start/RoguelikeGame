using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public interface ILoader<Key, Value> 
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Stats> StatsDict { get; private set; } = new Dictionary<int, Stats>();
    public Dictionary<int, EnemyStats> EnemyStatsDict { get; private set; } = new Dictionary<int, EnemyStats>();

    public void init()
    {
        StatsDict = LoadJson<StatData, int, Stats>("statData").MakeDict();
        EnemyStatsDict = LoadJson<EnemyStatData, int, EnemyStats>("testJson").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textasset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textasset.text);
    }

    public void ParseExcel()
    {
        EnemyStatData enemyStatData = new EnemyStatData();

        string[] lines = Managers.Resource.Load<TextAsset>("Data/EnemyTable").text.Split('\n');

        for(int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split('\t');
            if (row.Length == 0) continue;
            if (string.IsNullOrEmpty(row[0])) continue;

            enemyStatData.enemyStat.Add(new EnemyStats()
            {
                Em_ID = int.Parse(row[0]),
                Em_name = row[1],
                Em_MinAp = int.Parse(row[2]),
                Em_MaxAp = int.Parse(row[3]),
                Em_Hp = int.Parse(row[4]),
                Em_Jp = int.Parse(row[5]),
                Em_MoveSpd = float.Parse(row[6])
            });
        }
        
        string json = JsonUtility.ToJson(enemyStatData, true);
        File.WriteAllText($"{Application.dataPath}/Resources/Data/testJson.json", json);
        AssetDatabase.Refresh();
    }
}
