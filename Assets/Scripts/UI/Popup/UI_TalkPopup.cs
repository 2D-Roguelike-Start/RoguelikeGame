using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TalkPopup : UI_Popup
{
    public string Name;
    public string Talk;

    enum Texts
    {
        Name,
        Talk,
        Confirm,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));

        GetText((int)Texts.Name).text = Name;
        StartCoroutine("DialogText");
    }

    IEnumerator DialogText()
    {
        for(int i = 0; i < Talk.Length; i++)
        {
            GetText((int)Texts.Talk).text += Talk[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        GetText((int)Texts.Confirm).text = "È®ÀÎ(F)";
    }
}
