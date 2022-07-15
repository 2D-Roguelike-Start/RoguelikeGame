using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeginData : MonoBehaviour
{
    private void Awake()
    {
        ParseExcel();
    }

    void ParseExcel()
    {
        EnemyStatData enemyStatData = new EnemyStatData();

        //텍스트파일을 string배열에 줄단위로 저장
        string[] lines = Managers.Resource.Load<TextAsset>("Data/EnemyTable").text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            //row 에 탭단위로 저장
            string[] row = lines[i].Split('\t');
            if (row.Length == 0) continue;
            if (string.IsNullOrEmpty(row[0])) continue;

            //데이터 필요한 클래스 List에 값 Add
            enemyStatData.enemyStat.Add(new EnemyStats()
            {
                Em_ID = int.Parse(row[0]),
                Em_name = row[1],
                Em_MinAp = int.Parse(row[2]),
                Em_MaxAp = int.Parse(row[3]),
                Em_Hp = int.Parse(row[4]),
            });
        }

        // 직렬화 된 데이터를 json화 시켜서 문자열에 저장
        string json = JsonUtility.ToJson(enemyStatData, true);
        //json파일 원하는위치에 생성
        File.WriteAllText($"{Application.dataPath}/Resources/Data/EnemyTableJson.json", json);
    }
}
