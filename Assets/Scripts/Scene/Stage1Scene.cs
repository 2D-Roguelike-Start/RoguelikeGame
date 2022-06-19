using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Scene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Stage1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadScene(Define.Scene.Tutorial);
        }
    }

    public override void Clear()
    {
        Debug.Log("Stage1 Clear");
    }

}
