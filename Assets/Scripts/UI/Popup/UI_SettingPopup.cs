using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingPopup : UI_Popup
{
    enum Buttons
    {
        Setting_Screen,
        Setting_Sound,
        Exit,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Setting_Screen).gameObject.BindEvent(OnClickSetting_ScreenButton);
        GetButton((int)Buttons.Setting_Sound).gameObject.BindEvent(OnClickSetting_SoundButton);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(OnClosePopup);

    }

    void OnClickSetting_ScreenButton()
    {
        Debug.Log("OnClickSetting_ScreenButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
        Managers.UI.ShowPopupUI<UI_Setting_ScreenPopup>();
    }

    void OnClickSetting_SoundButton()
    {
        Debug.Log("OnClickSetting_SoundButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
        Managers.UI.ShowPopupUI<UI_Setting_SoundPopup>();
    }

    void OnClosePopup()
    {
        Debug.Log("OnClosePopup");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick");
        Managers.UI.ClosePopupUI(this);
    }
}
