using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    //component�� ������ ã�����ؼ� �߰��ϴ±��
    public static T GetAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
}
