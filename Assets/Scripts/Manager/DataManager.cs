using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Data dictionary 인터페이스 정의
//ILoader라는 인터페이스는 Dictionary형식의 key, value 를 뱉어주는 MakeDict()를 반드시 구현
public interface ILoader<Key, Value> 
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    //필요한 데이터 있을 때마다 추가해줘야 하는 부분
    public Dictionary<int, Stats> StatsDict { get; private set; } = new Dictionary<int, Stats>();
    public Dictionary<int, EnemyStats> EnemyStatsDict { get; private set; } = new Dictionary<int, EnemyStats>();

    public void init()
    {
        StatsDict = LoadJson<StatData, int, Stats>("statData").MakeDict();
        EnemyStatsDict = LoadJson<EnemyStatData, int, EnemyStats>("EnemyTableJson").MakeDict();
    }

    //편의로 Loader(json파일을 파싱해서 뱉어줌) 을 뱉어주는 함수를만듬 제네릭과 비슷한 느낌 여기서 Loader은 StatData, EnemyStatData.
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textasset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        //JsonUtility.FromJson<Loader>(textasset.text) == JsonUtility.FromJson<(StatData) || (EnemyStatData)>(textasset.text)
        return JsonUtility.FromJson<Loader>(textasset.text);
    }
}
