using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    //get -> public, set -> protected
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    // ����� ������ �۵�
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        //UI �������� �ʼ� EVENTSYSTEM �߰�
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
    
}
