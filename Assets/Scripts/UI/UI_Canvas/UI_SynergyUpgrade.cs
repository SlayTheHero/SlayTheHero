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
        UI_CharacterListPanel, UI_CharacterListGridPanel, UI_SynergyUpgradeOutputPanel
    }

    enum Images
    {
        UI_BackGround, UI_Input_1, UI_Input_2, UI_Output_1, UI_Output_2, UI_Output_3
    }
    enum Texts
    {
    }
    enum Buttons
    {
        UI_StartButton, UI_Output_1, UI_Output_2, UI_Output_3
    }
    GameManager manager;
    List<GameObject> UnitList;
    UI_SynergyOutputPanel outPutSelect;

    protected override void Init()
    {
        manager = GameManager.getInstance();
        manager.UI.SetCanvas(this.gameObject, true);
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
         

        loadUnitDeque();

        GetImage((int)Images.UI_Input_1).gameObject.AddUIEvent(OnInputClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_Input_2).gameObject.AddUIEvent(OnInputClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_Output_1).gameObject.AddUIEvent(OnOutputClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_Output_2).gameObject.AddUIEvent(OnOutputClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_Output_3).gameObject.AddUIEvent(OnOutputClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_Input_1).color = Utility.DarkGrey;
        GetImage((int)Images.UI_Input_2).color = Utility.DarkGrey;
        
        GetButton((int)Buttons.UI_StartButton).gameObject.AddUIEvent(OnStartButtonClicked, UI_EventHandler.UIEvent.LClick);
        selectUnitIndex = (-1, -1);
        outPutSelect = GetGameObject((int)GameObjects.UI_SynergyUpgradeOutputPanel).GetComponent<UI_SynergyOutputPanel>();
        setOutput();
    }
    (int, int) selectUnitIndex = (-1, -1);
    UI_CharacterListPanel characterList;
    private void loadUnitDeque()
    {
        int unitCount = manager.PlayerData.unitDeque.GetUnitCount();
        GameObject UI_CharacterListPanel = GetGameObject((int)GameObjects.UI_CharacterListPanel);
        characterList = UI_CharacterListPanel.GetComponent<UI_CharacterListPanel>();

        characterList.LoadPlayerData();
        characterList.SetSynergy(true);
        characterList.SetUnitEvent(OnCharacterClicked, UI_EventHandler.UIEvent.LClick);
    }
    private void OnCharacterClicked(PointerEventData data)
    { 
        int index = int.Parse(data.selectedObject.name.Split("_")[2]);
        //빠지는 로직
        GameObject target = null; ;
        int unitIndex = 0;
        if(index == selectUnitIndex.Item1)
        {
            target = GetImage((int)Images.UI_Input_1).gameObject;
            unitIndex = selectUnitIndex.Item1;
        }
        else if (index == selectUnitIndex.Item2)
        {
            target = GetImage((int)Images.UI_Input_2).gameObject;
            unitIndex = selectUnitIndex.Item2;

        }
        if (target != null)
        { 
            PointerEventData pData = new PointerEventData(EventSystem.current);
            pData.selectedObject = target;
            ExecuteEvents.Execute(target, pData, ExecuteEvents.pointerClickHandler);
            characterList.SetUnitSelected(unitIndex, false);
            setOutput();
            return;
        }
        if (selectUnitIndex.Item1 != -1 && selectUnitIndex.Item2 != -1) return;
        if (selectUnitIndex.Item1 == -1)
        {
            selectUnitIndex = (index,selectUnitIndex.Item2);
            GetImage((int)Images.UI_Input_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Unit,manager.PlayerData.unitDeque.GetUnit(index).ID);
            GetImage((int)Images.UI_Input_1).color = Color.white; 
            characterList.SetUnitSelected(index, true);

            setOutput();
            return;
        }
        if (selectUnitIndex.Item2 == -1 )
        {
            selectUnitIndex = (selectUnitIndex.Item1, index);
            GetImage((int)Images.UI_Input_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, manager.PlayerData.unitDeque.GetUnit(index).ID);
            GetImage((int)Images.UI_Input_2).color = Color.white; 
            characterList.SetUnitSelected(index, true);

            setOutput();
            return;
        }
    }

    private void OnInputClicked(PointerEventData data)
    {
        int index = int.Parse(data.selectedObject.name.Split("_")[2]);
        if(index == 1)
        {
            if (selectUnitIndex.Item1 == -1) return;
             
            GetImage((int)Images.UI_Input_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetImage((int)Images.UI_Input_1).color = Utility.DarkGrey;
            characterList.SetUnitSelected(selectUnitIndex.Item1, false);
            selectUnitIndex = (-1, selectUnitIndex.Item2);
        }
        else
        {
            if (selectUnitIndex.Item2 == -1) return;
             
            GetImage((int)Images.UI_Input_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetImage((int)Images.UI_Input_2).color = Utility.DarkGrey;
            characterList.SetUnitSelected(selectUnitIndex.Item2, false);
            selectUnitIndex = (selectUnitIndex.Item1, -1);
        }
        setOutput();
    }
    private void setOutput()
    {
        if (selectUnitIndex.Item1 == -1 || selectUnitIndex.Item2 == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                GetImage((int)Images.UI_Output_1 + i).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
                GetImage((int)Images.UI_Output_1 + i).color = Utility.DarkGrey;
            }
            outPutSelect.Clear();
            isUpgradeReady = false;
            return;
        }
        UnitDeque deque = manager.PlayerData.unitDeque;
        UnitBase unit1 = deque.GetUnit(selectUnitIndex.Item1);
        UnitBase unit2 = deque.GetUnit(selectUnitIndex.Item2);
        
        List<int> sameSynergy = new List<int>();
        bool existSameSynergy = false;
        if (unit1.Job == unit2.Job)
        {
            sameSynergy.Add((int)unit1.Job + 4);
        }
        if (unit1.Feature == unit2.Feature)
        {
            sameSynergy.Add((int)unit1.Feature + 7); 
        }
        if (unit1.Race == unit2.Race)
        {
            sameSynergy.Add((int)unit1.Race); 
        }
        for (int i = 0; i < sameSynergy.Count; i++)
        {
            GetImage((int)Images.UI_Output_1 + i).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, sameSynergy[i]);
            GetImage((int)Images.UI_Output_1 + i).color = Color.white;
            existSameSynergy = true;
        }
        if (existSameSynergy)
        {
            for (int i = sameSynergy.Count; i < 3; i++)
            {
                GetImage((int)Images.UI_Output_1 + i).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
                GetImage((int)Images.UI_Output_1 + i).color = Utility.DarkGrey;
            }
            isUpgradeReady = true;
        }
        else
        {
            isUpgradeReady = false;
        }


    }
    bool isUpgradeReady = false;
    private void OnOutputClicked(PointerEventData data)
    {
        Debug.Log($"selectUnitIndex : {selectUnitIndex.Item1} , 2 : {selectUnitIndex.Item2}");
        if (selectUnitIndex.Item1 == -1 || selectUnitIndex.Item2 == -1)
        {
            return;
        }
        if (!isUpgradeReady) return;

        int index = int.Parse(data.selectedObject.name.Split("_")[2]) - 1;
        Debug.Log($"index : {index} , nowSelect : {outPutSelect.nowSelect}");
        if (index != outPutSelect.nowSelect) return;

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
        GetImage((int)Images.UI_Input_1).color = Utility.DarkGrey;
        GetImage((int)Images.UI_Input_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
        GetImage((int)Images.UI_Input_2).color = Utility.DarkGrey;
        setOutput();
    }

    private void OnStartButtonClicked(PointerEventData data)
    {
        SaveManager.SaveData(manager.PlayerData,0);
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
