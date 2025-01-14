using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitRecruit_Reward : UI_Base
{
    enum GameObjects
    {
        UI_CharacterListPanel, UI_CharacterListGridPanel, UI_CharacterDialoguePanel, UI_Cover
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
        UI_StartButton, UI_RecruitSlot_1, UI_RecruitSlot_2, UI_RecruitSlot_3
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

        UnitBase unit = manager.PlayerData.unitDeque.GetUnit(manager.PlayerData.unitDeque.GetUnitCount() - 1);
        GetGameObject((int)GameObjects.UI_Cover).AddUIEvent(OnCoverClicked, UI_EventHandler.UIEvent.LClick);
        GetGameObject((int)GameObjects.UI_CharacterDialoguePanel).GetComponent<UI_CharacterDialoguePanel>().SetCharacterData(unit);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void OnCoverClicked(PointerEventData data)
    {
        manager.UI.ClosePopupUI();
    }
}
     
