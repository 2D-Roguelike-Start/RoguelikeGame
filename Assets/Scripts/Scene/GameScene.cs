using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Tutorial;

        Managers.UI.ShowSceneUI<UI_Inven>();
    }

    public override void Clear()
    {
       
    }

}
