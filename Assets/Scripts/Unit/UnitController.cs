using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<UnitState> UnitStateChange;
    public UnitBase Unit;

    [SerializeField]
    float _default_scale = 2f;
    [SerializeField]
    float _targetable_scale = 2.5f;

    UnitState _state;
    public enum UnitState
    {
        Default, Attacker, Target, Targetable, Untargetable, Hit
    }

    private void Awake()
    {
        _state = UnitState.Default;
    }

    public void MoveFront()
    {
        if (Unit.IsPlayerUnit)
        {
            transform.DOLocalMoveX(transform.localPosition.x + 1.5f, 0.5f);

        }
        else
        {
            transform.DOLocalMoveX(transform.localPosition.x - 1.5f, 0.5f);
        }
    }

    public void Scale(float x, float y, float duration)
    {
        transform.DOScale(new Vector2(x, y), duration);
    }
    public void SetState(UnitState state)
    {
        _state = state;
        //transform.localScale = new Vector3(_default_scale, _default_scale, 1f);
        switch (state)
        {
            case UnitState.Default:
                Scale(_default_scale, _default_scale, 0.2f);
                ChageAlpha(1f);
                break;

            case UnitState.Target:

                Scale(_default_scale, _default_scale, 0.2f);
                ChageAlpha(1f);
                break;
            case UnitState.Targetable:
                ChageAlpha(1f);
                break;
            case UnitState.Untargetable:
                ChageAlpha(0.5f);
                break;
            case UnitState.Attacker:
                ChageAlpha(1f);
                break;

            default:
                break;
        }
        UnitStateChange.Invoke(state);
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (_state == UnitState.Targetable)
        {
            BattleManager.Instance.SkillUsed.Invoke(Unit);
            SetState(UnitState.Target);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_state == UnitState.Targetable)
        {
            Scale(_targetable_scale, _targetable_scale, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_state == UnitState.Targetable)
        {
            Scale(_default_scale, _default_scale, 0.5f);
        }
    }
    void ChageAlpha(float alpha)
    {
        var sr = GetComponentsInChildren<SpriteRenderer>();
        foreach (var c in sr)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, alpha);
        }
    }
}
