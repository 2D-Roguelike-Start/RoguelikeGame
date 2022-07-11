using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title : UI_Scene
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
        //GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton());


    }

    void OnClickStartButton()
    {
        Debug.Log("OnClickStartButton");
        Managers.Scene.LoadScene(Define.Scene.Tutorial);
    }

    void OnClickSettingButton()
    {
        Debug.Log("OnClickSettingButton");
    }
}
