using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_IntroScene : UI_Base
{
    enum GameObjects
    {
    }

    enum Images
    {
        UI_StartButton, UI_SettingButton
    }
    enum Texts
    {
    }
    enum Buttons
    {
        UI_StartButton, UI_SettingButton    
    }
    GameManager manager;
    protected override void Init()
    {
        manager = GameManager.getInstance();
        manager.UI.SetCanvas(this.gameObject, false);
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.UI_StartButton).gameObject.AddUIEvent((p) => SceneController.ChangeScene(SceneController.SceneType.SaveSelect),UI_EventHandler.UIEvent.LClick);
        GetButton((int)Buttons.UI_SettingButton).gameObject.AddUIEvent((p) => manager.UI.ShowPopupUI<UI_Setting>(), UI_EventHandler.UIEvent.LClick);

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
}
