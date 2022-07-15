using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneManagerExtended
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    //현재 씬 제거, Define 정의된 scene 로드
    public void LoadScene(Define.Scene type)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    //reflection 이용, 이름추출
    string GetSceneName(Define.Scene type)
    {
        string name = Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    //현재 씬 제거
    public void Clear()
    {
        CurrentScene.Clear();
    }
}
