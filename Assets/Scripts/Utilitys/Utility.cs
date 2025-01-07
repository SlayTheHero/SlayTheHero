using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Utility
{
    public static Color DarkGrey = new Color32(120, 120, 120, 255); 

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
    /// 
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

    public static object KoreanToEnum(string korean)
    {
        string enumString = "";
        switch (korean)
        {
            case "검사":
                enumString = "Job_SwordMan";
                break;
            case "궁사":
                enumString =  "Job_Archer";
                break;
            case "마법사":
                enumString =  "Job_Magician";
                break;
            case "신속":
                enumString =  "Feature_Swiftness";
                break;
            case "의심암귀":
                enumString =  "Feature_SuspiciousGhost";
                break;
            case "나태":
                enumString =  "Feature_Sloth";
                break;
            case "질투":
                enumString =  "Feature_Envy";
                break;
            case "뱀파이어":
                enumString =  "Race_Vampire";
                break;
            case "마수":
                enumString =  "Race_DemonBeast";
                break;
            case "몽마":
                enumString =  "Race_NightMare";
                break;
            case "유령":
                enumString =  "Race_Ghost";
                break;
            default:
                return "";
        }
        object enumValue = ConvertStringToEnum(enumString);
        return enumValue;
    }

    private static object ConvertStringToEnum(string input)
    {
        // 문자열에서 Enum 이름과 값 분리
        var parts = input.Split('_');
        if (parts.Length != 2)
        {
            Debug.LogError("Invalid input format.");
            return null;
        }

        string enumType = parts[0];
        string enumValue = parts[1];

        // Enum 타입 찾기
        System.Type targetType = System.Type.GetType(enumType);
        if (targetType == null || !targetType.IsEnum)
        {
            Debug.LogError($"Enum type {enumType} does not exist.");
            return null;
        }

        // Enum 값 변환
        if (System.Enum.TryParse(targetType, enumValue, out var result))
        {
            return result;
        }

        Debug.LogError($"Value {enumValue} is not valid for enum {enumType}.");
        return null;
    }


}
