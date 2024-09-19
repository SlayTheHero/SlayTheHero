using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class SceneController
{
    public static GameObject LoadingUIPrefab;
    public static GameObject LoadingUI;
    public static UI_Loading UI_LoadingClass;

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
        UI_LoadingClass.StartLoading(sceneType);
    } 
    public static void Initialize()
    {
        if(LoadingUIPrefab == null)
        {
            LoadingUIPrefab = Resources.Load<GameObject>("Prefabs/UI/Canvas/UI_Loading"); 
        }
        if(LoadingUI == null)
        {
            LoadingUI = GameObject.Instantiate(LoadingUIPrefab);
            LoadingUI.SetActive(false);
            UI_LoadingClass = LoadingUI.GetComponent<UI_Loading>();
            GameObject.DontDestroyOnLoad(LoadingUI);
        }
    }


}
