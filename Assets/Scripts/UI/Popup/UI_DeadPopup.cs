using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeadPopup : UI_Popup
{
    enum Texts
    {
        YOUDIED,
    }

    enum Buttons
    {
        RestartButton,
        goBackMainTitleButton,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        StartCoroutine("Youdied");
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.RestartButton).gameObject.BindEvent(OnClickRestartButton);
        GetButton((int)Buttons.goBackMainTitleButton).gameObject.BindEvent(OnClickgoBackMainTitleButton);
    }

    IEnumerator Youdied()
    {
        Bind<Text>(typeof(Texts));
        float fadeCount = 0f;
        while (fadeCount < 1.0f)
        {
            Debug.Log("start coroutine");
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            GetText((int)Texts.YOUDIED).color = new Color(GetText((int)Texts.YOUDIED).color.r, GetText((int)Texts.YOUDIED).color.g, GetText((int)Texts.YOUDIED).color.b, fadeCount);
        }
    }

    void OnClickRestartButton()
    {
        Debug.Log("OnClickRestartButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
        Managers.Scene.Clear();
        Managers.Scene.LoadScene(Define.Scene.Tutorial);
    }

    void OnClickgoBackMainTitleButton()
    {
        Debug.Log("OnClickgoBackMainTitleButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_UIButtonClick", UI_Setting_SoundPopup.EffectSound);
        Managers.Scene.LoadScene(Define.Scene.Title);
    }
}
