using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public Action OnClickHandler = null;
    public Action OnBeginDragHandler = null;
    public Action OnDragHandler = null;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnClick!");

        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag!");

        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag!");

        if (OnDragHandler != null)
            OnDragHandler.Invoke();
    }

    
}
