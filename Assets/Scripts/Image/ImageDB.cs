using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ImageDB 
{
    public enum ImageType
    {
        Unit,
        Skill,
        Synergy,
        Default,
    }

    /// <summary>
    /// �ش� Ÿ���� id�� �ش��ϴ� �̹����� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Sprite GetImage(ImageType type, int id)
    {
        if(!dict.ContainsKey(type))
        {
            initialize(type);
        }

        if (!dict[type].ContainsKey(id)) 
        {
            Debug.Log($"There is no {type} Image for id == {id}");
            return null;
        }

        return dict[type][id];
    }

    private static Dictionary<ImageType, Dictionary<int, Sprite>> dict = new Dictionary<ImageType, Dictionary<int, Sprite>>();
    private static void initialize(ImageType type)
    {
        if (type == ImageType.Default)
        {
            dict[type] = new Dictionary<int, Sprite>();
            Sprite spr = Resources.Load<Sprite>("Images/Default.png");
            dict[type].Add(0, spr);
            return;
        }
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Images/{type.ToString()}");
        dict[type] = new Dictionary<int, Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite temp = sprites[i];
            int id = int.Parse(temp.name.Split("_")[1]);
            dict[type].Add(id, temp);
        }
    }


}
