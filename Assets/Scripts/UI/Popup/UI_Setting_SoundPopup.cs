using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting_SoundPopup : UI_Popup
{
    public static float BgmSound = 1f;
    public static float EffectSound = 1f; 

    enum Scrollbars
    {
        BackgroundScroll,
        EffectsoundScroll,
    }

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
        Bind<Scrollbar>(typeof(Scrollbars));

        GetButton((int)Buttons.Exit).gameObject.BindEvent(OnClosePopup);
        GetScroll((int)Scrollbars.BackgroundScroll).gameObject.BindEvent(OnDragBackgroundScroll, Define.UIEvent.Drag);
        GetScroll((int)Scrollbars.EffectsoundScroll).gameObject.BindEvent(OnDragEffectsoundScroll, Define.UIEvent.Drag);

        GetScroll((int)Scrollbars.BackgroundScroll).value = Managers.Sound.GetVolume(Define.Sound.Bgm);
        GetScroll((int)Scrollbars.EffectsoundScroll).value = Managers.Sound.GetVolume(Define.Sound.Effect);
        
    }

    void OnDragBackgroundScroll()
    {
        Debug.Log("OnDragBackgroundScroll");
        Managers.Sound.SetVolume(Define.Sound.Bgm, GetScroll((int)Scrollbars.BackgroundScroll).value);
        BgmSound = GetScroll((int)Scrollbars.BackgroundScroll).value;
    }

    void OnDragEffectsoundScroll()
    {
        Debug.Log("OnDragEffectsoundScroll");
        Managers.Sound.SetVolume(Define.Sound.Effect, GetScroll((int)Scrollbars.EffectsoundScroll).value);
        EffectSound = GetScroll((int)Scrollbars.EffectsoundScroll).value;
    }

    void OnClosePopup()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", EffectSound);
        Managers.UI.ClosePopupUI(this);
    }

    private void Update()
    {
        
    }
}
