using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Data dictionary �������̽� ����
//ILoader��� �������̽��� Dictionary������ key, value �� ����ִ� MakeDict()�� �ݵ�� ����
public interface ILoader<Key, Value> 
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    //�ʿ��� ������ ���� ������ �߰������ �ϴ� �κ�
    public Dictionary<int, Stats> StatsDict { get; private set; } = new Dictionary<int, Stats>();
    public Dictionary<int, EnemyStats> EnemyStatsDict { get; private set; } = new Dictionary<int, EnemyStats>();

    public void init()
    {
        StatsDict = LoadJson<StatData, int, Stats>("statData").MakeDict();
        EnemyStatsDict = LoadJson<EnemyStatData, int, EnemyStats>("EnemyTableJson").MakeDict();
    }

    //���Ƿ� Loader(json������ �Ľ��ؼ� �����) �� ����ִ� �Լ������� ���׸��� ����� ���� ���⼭ Loader�� StatData, EnemyStatData.
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textasset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        //JsonUtility.FromJson<Loader>(textasset.text) == JsonUtility.FromJson<(StatData) || (EnemyStatData)>(textasset.text)
        return JsonUtility.FromJson<Loader>(textasset.text);
    }
}
