using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SynergyUpgrade : UI_Base
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
    List<GameObject> UnitList;
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

        loadUnitDeque();

        GetImage((int)Images.UI_Input_1).gameObject.AddUIEvent(OnInputClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_Input_2).gameObject.AddUIEvent(OnInputClicked, UI_EventHandler.UIEvent.LClick);
        GetButton((int)Buttons.UI_Output).gameObject.AddUIEvent(OnOutputClicked, UI_EventHandler.UIEvent.LClick);
        GetButton((int)Buttons.UI_Input_1).interactable = false;
        GetButton((int)Buttons.UI_Input_2).interactable = false;

        GetButton((int)Buttons.UI_StartButton).gameObject.AddUIEvent(OnStartButtonClicked, UI_EventHandler.UIEvent.LClick);
        selectUnitIndex = (-1, -1);

    }
    (int, int) selectUnitIndex = (-1, -1);

    private void loadUnitDeque()
    {
        int unitCount = manager.PlayerData.unitDeque.GetUnitCount();
        GameObject UI_CharacterListPanel = GetGameObject((int)GameObjects.UI_CharacterListPanel);
        UI_CharacterListPanel ui = UI_CharacterListPanel.GetComponent<UI_CharacterListPanel>();

        ui.LoadPlayerData();
        ui.SetUnitEvent(OnCharacterClicked, UI_EventHandler.UIEvent.LClick);
    }
    private void OnCharacterClicked(PointerEventData data)
    {
        if (selectUnitIndex.Item1 != -1 && selectUnitIndex.Item2 != -1) return;
        int index = int.Parse(data.pointerClick.name.Split("_")[2]);
        if (selectUnitIndex.Item1 == index) return;
        if (selectUnitIndex.Item1 == -1)
        {
            selectUnitIndex = (index,selectUnitIndex.Item2);
            GetImage((int)Images.UI_Input_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Unit,manager.PlayerData.unitDeque.GetUnit(index).ID);
            GetButton((int)Buttons.UI_Input_1).interactable = true;
            setOutput();
            return;
        }
        if (selectUnitIndex.Item2 == index) return;
        if (selectUnitIndex.Item2 == -1 )
        {
            selectUnitIndex = (selectUnitIndex.Item1, index);
            GetImage((int)Images.UI_Input_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, manager.PlayerData.unitDeque.GetUnit(index).ID);
            GetButton((int)Buttons.UI_Input_2).interactable = true;
            setOutput();
            return;
        }
    }

    private void OnInputClicked(PointerEventData data)
    {
        int index = int.Parse(data.pointerClick.name.Split("_")[2]);
        if(index == 1)
        {
            if (selectUnitIndex.Item1 == -1) return;
             
            GetImage((int)Images.UI_Input_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetButton((int)Buttons.UI_Input_1).interactable = false;
            selectUnitIndex = (-1, selectUnitIndex.Item2);
        }
        else
        {
            if (selectUnitIndex.Item2 == -1) return;
             
            GetImage((int)Images.UI_Input_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetButton((int)Buttons.UI_Input_2).interactable = false;
            selectUnitIndex = (selectUnitIndex.Item1, -1);
        }
        setOutput();
    }
    private void setOutput()
    {
        if (selectUnitIndex.Item1 == -1 || selectUnitIndex.Item2 == -1)
        { 
            GetImage((int)Images.UI_Output).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetButton((int)Buttons.UI_Output).interactable = false;
            return;
        }
        UnitDeque deque = manager.PlayerData.unitDeque;
        UnitBase unit1 = deque.GetUnit(selectUnitIndex.Item1);
        UnitBase unit2 = deque.GetUnit(selectUnitIndex.Item2);
        
        if (unit1.Job == unit2.Job)
        {
            GetImage((int)Images.UI_Output).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy,(int)unit1.Job + 4);
            GetButton((int)Buttons.UI_Output).interactable = true;
            return;
        }
        if (unit1.Feature == unit2.Feature)
        {
            GetImage((int)Images.UI_Output).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)unit1.Feature + 7);
            GetButton((int)Buttons.UI_Output).interactable = true;
            return;
        }
        if (unit1.Race == unit2.Race)
        {
            GetImage((int)Images.UI_Output).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)unit1.Race);
            GetButton((int)Buttons.UI_Output).interactable = true;
            return;
        }
        GetImage((int)Images.UI_Output).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
        GetButton((int)Buttons.UI_Output).interactable = false;
    }

    private void OnOutputClicked(PointerEventData data)
    {
        if (GetButton((int)Buttons.UI_Output).interactable == false) return;
        int firstIndex;
        int secondIndex;
        if(selectUnitIndex.Item1 < selectUnitIndex.Item2) 
        {
            firstIndex = selectUnitIndex.Item2;
            secondIndex = selectUnitIndex.Item1;
        }
        else
        {
            firstIndex = selectUnitIndex.Item1;
            secondIndex = selectUnitIndex.Item2;
        }
        manager.PlayerData.unitDeque.DeleteUnit(firstIndex);
        manager.PlayerData.unitDeque.DeleteUnit(secondIndex);
        loadUnitDeque();
        selectUnitIndex = (-1, -1);
         
        GetImage((int)Images.UI_Input_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
        GetButton((int)Buttons.UI_Input_1).interactable = false;
        GetImage((int)Images.UI_Input_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
        GetButton((int)Buttons.UI_Input_2).interactable = false;
        setOutput();
    }

    private void OnStartButtonClicked(PointerEventData data)
    {
        manager.UI.ClosePopupUI();
    }
    public void tempEvent(PointerEventData data)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
}
