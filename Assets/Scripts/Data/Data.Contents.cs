using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region Stats

[Serializable]
public class Stats
{
    public int level;
    public int hp;
    public int attack;
}

[Serializable]
public class StatData : ILoader<int, Stats>
{
    public List<Stats> stats = new List<Stats>();

    public Dictionary<int, Stats> MakeDict()
    {
        Dictionary<int, Stats> dict = new Dictionary<int, Stats>();
        foreach (Stats stat in stats)
            dict.Add(stat.level, stat);
        return dict;
    }
}
#endregion

#region EnemyStats
[Serializable]
public class EnemyStats
{
    public int Em_ID;
    public string Em_name;
    public int Em_MinAp;
    public int Em_MaxAp;
    public int Em_Hp;
    public int Em_Jp;
    public float Em_MoveSpd;
}

[Serializable]
public class EnemyStatData : ILoader<int, EnemyStats>
{
    public List<EnemyStats> enemyStat = new List<EnemyStats>();

    public Dictionary<int, EnemyStats> MakeDict()
    {
        Dictionary<int, EnemyStats> dict = new Dictionary<int, EnemyStats>();
        foreach (EnemyStats stat in enemyStat)
            dict.Add(stat.Em_ID, stat);
        return dict;
    }
}
#endregion