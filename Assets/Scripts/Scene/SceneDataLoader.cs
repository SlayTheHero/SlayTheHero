using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SceneDataLoader 
{
    int maxProgress = 0;
    int nowProgress = 0;
    public float progress { get {
            if (maxProgress == 0) return 1;
            return (float)nowProgress / (float)maxProgress; }}
    public bool isDone { get; private set; }
    
    public async void StartDataLoad(SceneController.SceneType type)
    {
        initialize();
        nowProgress = 0;
        isDone = false;
        await func.Invoke(type);
        isDone = true;
        maxProgress = 0;
    }

    delegate Task SceneDataLoadFunc(SceneController.SceneType type);
    SceneDataLoadFunc func;

    private bool isInitialized = false;
    private void initialize()
    {
        if(!isInitialized)
        {
            func = null;
            func += MainSceneDataLoader;


            isInitialized = true;
        }
    }

    private async Task MainSceneDataLoader(SceneController.SceneType type)
    {
        if(type == SceneController.SceneType.Main)
        {

        }
    }
}
