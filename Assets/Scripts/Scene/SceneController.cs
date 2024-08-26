using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class SceneController
{
    public static GameObject LoadingUIPrefab;
    public static GameObject LoadingUI;

    public enum SceneType
    {
        Main,
        SaveSelect,
        Dialogue,
        CharacterSelect,
        Maintenance,
        Battle,
    }

    public static void ChangeScene(SceneType sceneType)
    {
        Initialize();
        LoadingUI.GetComponent<UI_Loading>().StartLoading(sceneType);
    }

    private static void Initialize()
    {
        if(LoadingUI == null)
        {
            LoadingUIPrefab = Resources.Load<GameObject>("Prefabs/UI/Canvas/UI_Loading"); 
        }
        LoadingUI = GameObject.Find("UI_Loading");
        if(LoadingUI == null)
        {
            LoadingUI = GameObject.Instantiate(LoadingUIPrefab);
            GameObject.DontDestroyOnLoad(LoadingUI);
        }
    }


}
