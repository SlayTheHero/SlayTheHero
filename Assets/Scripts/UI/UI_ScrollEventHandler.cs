using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ScrollEventHandler : UI_EventHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnDragBeginHandler = null;
    public Action<PointerEventData> OnDragEndHandler = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnDragBeginHandler != null)
            OnDragBeginHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnDragEndHandler != null)
            OnDragEndHandler.Invoke(eventData);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
