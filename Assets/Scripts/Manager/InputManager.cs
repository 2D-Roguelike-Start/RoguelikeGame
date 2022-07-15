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
    public Action TouchAction = null;

    //click�� press�� ���� , Define.MouseEvent�� ����
    bool pressed = false;

    public void OnUpdate()
    {

        //UI �����̸� return
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI �Է��̵��Խ��ϴ�.");
            return;
        }

        //Ű���� Ű ���� ����
        if (KeyAction != null && NonKeyAction != null)
        {
            if (Input.anyKey)
                KeyAction.Invoke();
            else NonKeyAction.Invoke();
        }
        //���콺 �׼� ���� ����
        if(MouseAction != null)
        {
            //Press(�巡�׶� ����)
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

        //��ġ �׼� ���� ����
        if(TouchAction != null)
        {
            if(Input.touchCount > 0)
            {
                Debug.Log("touch action");
                TouchAction.Invoke();
            }
        }
    }

    //�Է� �ʱ�ȭ
    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
        TouchAction = null;
    }
}
