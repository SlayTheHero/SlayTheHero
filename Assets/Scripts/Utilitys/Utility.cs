using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Utility
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T compo = go.GetComponent<T>();
        if (compo == null)
        {
            compo = go.AddComponent<T>();
        }

        return compo;
    }
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recursive)
        {
            foreach (T compo in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || compo.name == name)
                {
                    return compo;
                }
            }
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T compo = transform.GetComponent<T>();
                    if (compo != null) return compo;
                }
            }
        }
        return null;
    }
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) return null;

        return transform.gameObject;
    }


    /// <summary>
    /// 문자열을 Enum 값에 맞게 파싱해줍니다.
    /// </summary>
    /// <typeparam name="T"> Enum 클래스</typeparam>
    /// <param name="enumName"> Enum 내부의 값의 문자열</param>
    /// <returns></returns>
    public static T StringToEnum<T>(string enumName) where T : struct, Enum
    {
        try
        {
            if (Enum.TryParse(enumName, true, out T enumValue))
            {
                return enumValue;
            }
            else
            {
                throw new ArgumentException($"'{enumName}' is not a valid name for enum '{typeof(T).Name}'.");
            }
        }
        catch (ArgumentException ex)
        {
            Debug.Log(ex.Message);
            return default;
        }
    }
}
