using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterSelect : UI_Base
{
    enum GameObjects
    {
        UI_CharaterListScrollBar,
        UI_CharacterListGridPanel,
        UI_CharacterDialoguePanel,
        UI_SkillGridPanel,
        UI_CharacterSelectGirdPanel
    }

    enum Images
    {
        UI_Character_1, UI_Character_2, UI_Character_3, UI_Character_4, UI_Character_5, UI_Character_6, UI_Character_7, UI_Character_8,
        UI_RaceIcon, UI_ClassIcon, UI_TraitIcon, UI_Portrait,
        UI_Skill_1, UI_Skill_2, UI_Skill_3, UI_Skill_4, 
        UI_CharacterSlot_1, UI_CharacterSlot_2, UI_CharacterSlot_3, 
        UI_StartButton,UI_SettingButton
    }
    enum Texts
    {
        UI_RaceText, UI_ClassText, UI_TraitText, UI_Name
    }
    enum Buttons
    {
        UI_StartButton, UI_SettingButton
    }

    GameManager manager;
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
        manager = GameManager.getInstance();

        for (int i = 0; i < 6; i++)
        {
            manager.PlayerData.unitDeque.AddUnit(UnitDB.GetUnit(i));
        }

        List<UnitBase> li = PlayerUnitContainer.GetUnitList();
        Console.WriteLine(li.Count);
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
