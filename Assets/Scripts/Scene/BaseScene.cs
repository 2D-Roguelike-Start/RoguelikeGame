using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    //get -> public, set -> protected
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    // 기능이 꺼져도 작동
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        //UI 구동관련 필수 EVENTSYSTEM 추가
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
    
}
