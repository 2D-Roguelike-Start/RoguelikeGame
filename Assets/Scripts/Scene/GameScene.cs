using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public Transform StartPoint;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Tutorial;
        //Managers.UI.ShowSceneUI<UI_Information>();
        Managers.UI.ShowSceneUI<UI_AndroidTouch>();

        GameObject.FindGameObjectWithTag("Player").gameObject.transform.position = StartPoint.position;
        PlayerStat.Hp = PlayerStat.MaxHp;
    }

    public override void Clear()
    {

    }

}
