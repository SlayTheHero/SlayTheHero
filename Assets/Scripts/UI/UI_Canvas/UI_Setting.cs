using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Base
{
    enum GameObjects
    {
    }

    enum Images
    {
    }
    enum Texts
    {
    }
    enum Buttons
    {
        UI_ExitButton, UI_SaveButton, UI_MainButton
    }
    GameManager manager;

    protected override void Init()
    {
        manager = GameManager.getInstance();
        manager.UI.SetCanvas(this.gameObject, true);
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.UI_ExitButton).gameObject.AddUIEvent((p) => OnExitButtonClicked(p), UI_EventHandler.UIEvent.LClick);
        
    }    
    void Start()
    {
        Init();
    }
    private void OnExitButtonClicked(PointerEventData data)
    { 
        manager.UI.ClosePopupUI();
    }
}
