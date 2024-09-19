using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitRecruit : UI_Base
{
    enum GameObjects
    {
        UI_CharacterListPanel, UI_CharacterListGridPanel
    }

    enum Images
    {
        UI_BackGround, UI_Output, UI_Input_1, UI_Input_2
    }
    enum Texts
    {
    }
    enum Buttons
    {
        UI_StartButton, UI_Output, UI_Input_1, UI_Input_2
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

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
}
     
