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