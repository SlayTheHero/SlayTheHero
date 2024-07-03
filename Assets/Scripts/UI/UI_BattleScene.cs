using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattleScene : UI_Base
{
    enum GameObjects
    {

    }

    enum Images
    {
        UI_TurnImage, UI_Turn_1, UI_Turn_2, UI_Turn_3, UI_Turn_4, UI_Turn_5, UI_Turn_6, UI_Turn_7, UI_Turn_8,
        UI_Player_1, UI_Player_2, UI_Player_3, UI_Player_4, UI_Enemy_1, UI_Enemy_2, UI_Enemy_3, UI_Enemy_4,
        UI_Portrait, UI_BaseAttack, UI_Skill_1, UI_Skill_2, UI_Skill_3, UI_SpecialSkill
    }
    enum Texts
    {
    }
    enum Buttons
    {
    }

    protected override void Init()
    {
        // GameManager.UI.SetCanvas(this.gameObject, true);
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        string[] names = Enum.GetNames(typeof(Images));
        for (int i = 0; i < names.Length; i++)
        {
            Image image = GetImage(i);
            image.gameObject.AddUIEvent(tempEvent, UI_EventHandler.UIEvent.LClick);
        }
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

    // Update is called once per frame
    void Update()
    {

    }
}
