using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Maintenance : UI_Base
{
    enum GameObjects
    {
    }

    enum Images
    {
        UI_Select_1, UI_Select_2, UI_Select_3, UI_Deque,
        UI_CharacterImage
    }
    enum Texts
    {
    }
    enum Buttons
    {
        UI_Select_1, UI_Select_2, UI_Select_3, UI_Deque
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

        string[] names = Enum.GetNames(typeof(Images));
        for (int i = 0; i < names.Length; i++)
        {
            Image image = GetImage(i);
            image.gameObject.AddUIEvent(tempEvent, UI_EventHandler.UIEvent.LClick);
        }
        GetButton((int)Buttons.UI_Select_1).gameObject.AddUIEvent((p) => manager.UI.ShowPopupUI<UI_SynergyUpgrade>(),UI_EventHandler.UIEvent.LClick);
    }
    public void tempEvent(PointerEventData data)
    {
        data.pointerClick.GetComponent<Image>().color = Color.red;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
}
