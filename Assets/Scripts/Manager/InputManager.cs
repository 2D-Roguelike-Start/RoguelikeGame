using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action NonKeyAction = null;

    public Action<Define.MouseEvent> MouseAction = null;

    bool pressed = false;

    public void OnUpdate()
    {
        //UI 관련이면 return
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI 입력이들어왔습니다.");
            return;
        }
            

        //키보드 키 감지 정의
        if (Input.anyKey)
            KeyAction.Invoke();
        else NonKeyAction.Invoke();

        //마우스 액션 감지 정의
        if(MouseAction != null)
        {
            //Press(드래그랑 관련)
            if (Input.GetMouseButton(0))
            {
                Debug.Log("mouse action");
                MouseAction.Invoke(Define.MouseEvent.Press);
                pressed = true;
            }
            //Click
            else
            {
                if (pressed)
                    MouseAction.Invoke(Define.MouseEvent.Click);
                pressed = false;
            }

        }
    }
}
