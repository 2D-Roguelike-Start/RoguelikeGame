using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting_SoundPopup : UI_Popup
{
    enum Buttons
    {
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

        GetButton((int)Buttons.Exit).gameObject.BindEvent(OnClosePopup);
    }

    void OnClosePopup()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
