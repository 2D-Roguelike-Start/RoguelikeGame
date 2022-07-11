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

        Managers.Data.ParseExcel();
        Dictionary<int, EnemyStats> dict = Managers.Data.EnemyStatsDict;
        Dictionary<int, Stats> dict2 = Managers.Data.StatsDict;
    }

    public override void Clear()
    {

    }
}
