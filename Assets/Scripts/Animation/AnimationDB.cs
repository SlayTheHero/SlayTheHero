using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationDB
{
    public const string PATH = "Animation/";
    public enum AnimType
    {
        UnitMotion,
        Effect
    }

    /// <summary>
    /// 해당 타입의 id에 해당하는 이미지를 반환합니다.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static AnimationClip GetAnimationClip(AnimType type, string name)
    {
        if (!dict.ContainsKey(type))
        {
            initialize(type);
        }

        if (!dict[type].ContainsKey(name))
        {
            Debug.Log($"There is no {type} Image for id == {name}");
            return null;
        }

        return dict[type][name];
    }

    private static Dictionary<AnimType, Dictionary<string, AnimationClip>> dict = new Dictionary<AnimType, Dictionary<string, AnimationClip>>();
    private static void initialize(AnimType type)
    {
        AnimationClip[] AnimationClips = Resources.LoadAll<AnimationClip>($"{PATH}{type.ToString()}");
        dict[type] = new Dictionary<string, AnimationClip>();
        for (int i = 0; i < AnimationClips.Length; i++)
        {
            AnimationClip temp = AnimationClips[i];
            dict[type].Add(temp.name, temp);
        }
    }

}
