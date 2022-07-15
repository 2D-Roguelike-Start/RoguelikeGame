using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Information : UI_Scene
{
    PlayerController Player;

    enum Images
    {
        HPBarBack,
        HPBar,
        SkillIcon,
        ItemPanel,
    }

    enum Buttons
    {
        Setting,
    }

    enum Texts
    {
        HP,
    }

    void Start()
    {
        
        
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        //GetImage((int)Images.HPBarBack);
        ////GetImage((int)Images.HPBar);
        //GetImage((int)Images.SkillIcon);
        ////GetText((int)Texts.HP).text = $"{playerstat.Hp}";
        //GetImage((int)Images.Test);

        GetButton((int)Buttons.Setting).gameObject.BindEvent(OnClickSetting);

        Managers.Sound.Clear();
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_Tutorial", UI_Setting_SoundPopup.BgmSound);
    }

    void OnClickSetting()
    {
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    private void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        GetText((int)Texts.HP).text = $"{PlayerStat.Hp}";
        GetImage((int)Images.HPBar).fillAmount = Mathf.Lerp(GetImage((int)Images.HPBar).fillAmount, PlayerStat.Hp / PlayerStat.MaxHp, Time.deltaTime * 0.9f);
        //Player.ispossession = false;
    }
}
