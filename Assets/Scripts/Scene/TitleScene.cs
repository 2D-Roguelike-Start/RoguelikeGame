using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Title;
        Managers.UI.ShowPopupUI<UI_TitlePopup>();

    }

    public override void Clear()
    {

    }
}
