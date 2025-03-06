using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
                                              IPointerMoveHandler, IPointerDownHandler,  IPointerUpHandler
{
    public enum UIEvent
    {
        LClick,
        Enter,
        Exit,
        Hold
    }

    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnPointerEnterHandler = null;
    public Action<PointerEventData> OnPointerMoveHandler = null;
    public Action<PointerEventData> OnPointerExitHandler = null;
    public Action<PointerEventData> OnPointerDownHandler = null;
    public Action<PointerEventData> OnPointerUpHandler = null;
    public Action<PointerEventData> OnHoldHandler = null; // 클릭을 유지할 때 호출

    private Coroutine _holdCoroutine;
    private readonly float _holdThreshold = 0.5f; // 0.5초 이상 누르면 "Hold"로 판정

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterHandler?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitHandler?.Invoke(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        OnPointerMoveHandler?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke(eventData);

        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
        }

        _holdCoroutine = StartCoroutine(HoldCoroutine(eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke(eventData);

        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
        }
    }

    private IEnumerator HoldCoroutine(PointerEventData eventData)
    {
        yield return new WaitForSeconds(_holdThreshold); 

        OnHoldHandler?.Invoke(eventData); 
    }
}
