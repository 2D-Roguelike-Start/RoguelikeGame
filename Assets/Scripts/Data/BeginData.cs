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

        //�ؽ�Ʈ������ string�迭�� �ٴ����� ����
        string[] lines = Managers.Resource.Load<TextAsset>("Data/EnemyTable").text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            //row �� �Ǵ����� ����
            string[] row = lines[i].Split('\t');
            if (row.Length == 0) continue;
            if (string.IsNullOrEmpty(row[0])) continue;

            //������ �ʿ��� Ŭ���� List�� �� Add
            enemyStatData.enemyStat.Add(new EnemyStats()
            {
                Em_ID = int.Parse(row[0]),
                Em_name = row[1],
                Em_MinAp = int.Parse(row[2]),
                Em_MaxAp = int.Parse(row[3]),
                Em_Hp = int.Parse(row[4]),
            });
        }

        // ����ȭ �� �����͸� jsonȭ ���Ѽ� ���ڿ��� ����
        string json = JsonUtility.ToJson(enemyStatData, true);
        //json���� ���ϴ���ġ�� ����
        File.WriteAllText($"{Application.dataPath}/Resources/Data/EnemyTableJson.json", json);
    }
}
