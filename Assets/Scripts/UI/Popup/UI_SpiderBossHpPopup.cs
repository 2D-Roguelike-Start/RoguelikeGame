using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpiderBossHpPopup : UI_Popup
{
    BossController_Spider spiderBoss;

    enum Images
    {
        HPBar,
    }

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

    }

    private void Update()
    {
        spiderBoss = GameObject.FindGameObjectWithTag("Boss").GetAddComponent<BossController_Spider>();

        GetImage((int)Images.HPBar).fillAmount = Mathf.Lerp(GetImage((int)Images.HPBar).fillAmount, spiderBoss.stat.Hp / spiderBoss.stat.MaxHp, Time.deltaTime * 0.9f);
    }
}
