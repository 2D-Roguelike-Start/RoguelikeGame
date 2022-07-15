using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//UI 관련 이벤트 정의
public class UI_EventController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public Action OnClickHandler = null;
    public Action OnBeginDragHandler = null;
    public Action OnDragHandler = null;

    //클릭
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnClick!");

        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }
    //드래그시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag!");

        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke();
    }
    //드래그중
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag!");

        if (OnDragHandler != null)
            OnDragHandler.Invoke();
    }

    
}
