using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,IDeselectHandler
{
    public enum UIEvent
    {
        LClick,
        Enter,
        Exit,
        Deselect
    } 
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnPointerEnterHandler = null;
    public Action<PointerEventData> OnPointerMoveHandler = null;
    public Action<PointerEventData> OnPointerExitHandler = null;
    public Action<PointerEventData> OnDeselectHandler = null;

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
    public void OnDeselect(BaseEventData eventData)
    {
        if (OnDeselectHandler != null)
            OnDeselectHandler.Invoke(eventData as PointerEventData);
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
