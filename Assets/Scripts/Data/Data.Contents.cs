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
[Serializable] //외부에서 parsing 할수있게 함
public class EnemyStats
{
    //public 중요
    public int Em_ID;
    public string Em_name;
    public int Em_MinAp;
    public int Em_MaxAp;
    public int Em_Hp;
}

[Serializable]
public class EnemyStatData : ILoader<int, EnemyStats>
{
    public List<EnemyStats> enemyStat = new List<EnemyStats>();

    //인터페이스 구현 Dictionary<int, EnemyStats> return해주는 MakeDict()
    public Dictionary<int, EnemyStats> MakeDict()
    {
        Dictionary<int, EnemyStats> dict = new Dictionary<int, EnemyStats>();
        foreach (EnemyStats stat in enemyStat)
            dict.Add(stat.Em_ID, stat);
        return dict;
    }
}
#endregion