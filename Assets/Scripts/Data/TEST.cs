using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TEST : MonoBehaviour
{
    TextAsset TestText;

    private void Start()
    {
       // List<LoadExcel> list = new List<LoadExcel>();
        LoadExcel test = new LoadExcel();
        //Dictionary<int, LoadExcel> testdict = new Dictionary<int, LoadExcel>();
        TestText = Managers.Resource.Load<TextAsset>("Data/test");
        string[] str = TestText.text.Split('\n');
        string[] begin = str[1].Split('\t');
        test.ID = int.Parse(begin[0]);
        test.ATTACK = int.Parse(begin[1]);

        //foreach (LoadExcel load in list)
        //    testdict.Add(load.ID, load);

        string json = JsonUtility.ToJson(test);
        Debug.Log(json);
        string fileName = "jsonTest";
        string path = Application.dataPath + "/Resources/Data/" + fileName + ".json";

        File.WriteAllText(path, json);
    }
}
