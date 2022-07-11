using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitlePopup : UI_Popup
{
    enum GameObjects
    {

    }

    enum Images
    {
        Buttons,
    }

    enum Buttons
    {
        StartButton,
        SettingButton,
        ExitButton,
    }

    enum Texts
    {
        GameTitle,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        Managers.Sound.Clear();
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_MainTitle");
    }

    void OnClickStartButton()
    {
        Debug.Log("OnClickContinueButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
        Managers.Scene.LoadScene(Define.Scene.Tutorial);
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickSettingButton()
    {
        Debug.Log("OnClickSettingButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    void OnClickExitButton()
    {
        Debug.Log("OnClickExitButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
