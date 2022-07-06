using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadExcel
{
    //TextAsset TestText;

    ////private void Start()
    ////{
    ////    TestText = Managers.Resource.Load<TextAsset>("Data/test");
    ////    string[] Lines = TestText.text.Split('\n');
    ////    string[] Key = Lines[0].Split('\t');
    ////    Dictionary<string, string> testdict = new Dictionary<string, string>();
    ////    for (int i = 1; i < Lines.Length; i++)
    ////    {
    ////        string[] tab = Lines[i].Split('\t');
    ////        for(int j = 0; j < tab.Length; j++)
    ////        {
    ////            testdict.Add(Key[i - 1], tab[j]);
    ////        }
    ////    }

    ////    Debug.Log(testdict);
    ////}
    //public int ID { get; private set; }


    //private void Start()
    //{
    //    TestText = Managers.Resource.Load<TextAsset>("Data/test");
    //    string[] Lines = TestText.text.Split('\n');
    //    string[] Key = Lines[0].Split('\t');
    //}
    public int ID;
    public int ATTACK;

}
