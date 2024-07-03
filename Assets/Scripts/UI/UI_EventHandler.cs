using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public enum UIEvent
    {
        LClick,
        LDrag,
        Enter,
        Exit
    }
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnPointerEnterHandler = null;
    public Action<PointerEventData> OnPointerMoveHandler = null;
    public Action<PointerEventData> OnPointerExitHandler = null;

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterHandler != null)
            OnPointerEnterHandler.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnPointerExitHandler != null)
            OnPointerExitHandler.Invoke(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (OnPointerMoveHandler != null)
            OnPointerMoveHandler.Invoke(eventData);
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
