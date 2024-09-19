using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DeckButton : UI_EventHandler
{
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        OnClickHandler += OnDeckButtonClicked;
    }
    void OnDeckButtonClicked(PointerEventData data)
    {
        if (manager == null) manager = GameManager.getInstance();

        UI_CharacterSelect ui = manager.UI.ShowPopupUI<UI_CharacterSelect>();
        ui.setDeck(true);
    }
    // Update is called once per frame
    void Update()
    {

    }

}
