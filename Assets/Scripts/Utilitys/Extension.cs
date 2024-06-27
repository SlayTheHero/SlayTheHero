using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// extension method 클래스입니다
/// </summary>
public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T compo = go.GetComponent<T>();
        if (compo == null)
        {
            compo = go.AddComponent<T>();
        }

        return compo;
    }
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, UI_EventHandler.UIEvent type = UI_EventHandler.UIEvent.LClick)
    {
        UI_Base.BindUIEvent(go, action, type);
    }
}

