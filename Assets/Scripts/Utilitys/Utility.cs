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

}
