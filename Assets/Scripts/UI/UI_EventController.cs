using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//UI ���� �̺�Ʈ ����
public class UI_EventController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public Action OnClickHandler = null;
    public Action OnBeginDragHandler = null;
    public Action OnDragHandler = null;

    //Ŭ��
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnClick!");

        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }
    //�巡�׽���
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag!");

        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke();
    }
    //�巡����
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag!");

        if (OnDragHandler != null)
            OnDragHandler.Invoke();
    }

    
}
