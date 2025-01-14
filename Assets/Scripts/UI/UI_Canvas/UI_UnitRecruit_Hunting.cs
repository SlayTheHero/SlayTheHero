using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitRecruit_Hunting : UI_Base
{
    enum GameObjects
    {
        UI_LoadingCharacter, UI_CharacterListGridPanel
    }

    enum Images
    {

    }
    enum Texts
    {
        UI_LoadingText
    }
    enum Buttons
    {

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

    }

    void Start()
    {
        Init();
        StartCoroutine(CountDownCoroutine());
    }

    void OnCountEnd()
    {
        manager.UI.ClosePopupUI();
        manager.UI.ShowPopupUI<UI_UnitRecruit_Reward>();
    }

    IEnumerator CountDownCoroutine()
    {
        yield return new WaitForSeconds(3f); 
        OnCountEnd();
    }
}
     
