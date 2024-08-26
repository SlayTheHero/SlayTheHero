using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaveSelcet : UI_Base
{
    enum GameObjects
    { 
        UI_SaveSlot_1, UI_SaveSlot_2, UI_SaveSlot_3,
    }


    GameManager manager;
    protected override void Init()
    {
        // GameManager.UI.SetCanvas(this.gameObject, true);
        Bind<GameObject>(typeof(GameObjects));

        manager = GameManager.getInstance();


        manager.PlayerData.playerName = "Wdas";
        manager.PlayerData.playCount = 3;
        for (int i = 0; i < 6; i++)
        {
            manager.PlayerData.unitDeque.AddUnit(UnitDB.GetUnit(i));
        }
        List<UnitBase> li = PlayerUnitContainer.GetUnitList();
        Console.WriteLine(li.Count);
        for (int i = 0; i < 3; i++)
        {
            manager.PlayerData.unitDeque.AddUnit(li[i]);
        }
        SaveManager.SaveData(manager.PlayerData, 0);
        manager.PlayerData = SaveManager.LoadData(0);
        manager.PlayerData.playerName = "asf";
        manager.PlayerData.playCount = 4;
        SaveManager.SaveData(manager.PlayerData, 1);
        manager.PlayerData.playerName = "asfsa";
        manager.PlayerData.playCount = 7;

        GameObject saveSlotPanel = transform.GetChild(0).gameObject;
        for (int i = 0; i < SaveManager.MAX_SAVE_SLOT; i++)
        {
            PlayerData data = SaveManager.LoadData(i);
            if (data == null)
            {
                UpdateSaveUI(saveSlotPanel.transform.GetChild(i).gameObject, data, true);
            }
            else
            {
                UpdateSaveUI(saveSlotPanel.transform.GetChild(i).gameObject, data, false);
            }
        }

        Debug.Log("SaveLoad Complete");


    }
    private void UpdateSaveUI(GameObject SaveSlot,PlayerData data,bool isDisabled)
    {
        if(isDisabled)
        {
            SaveSlot.transform.Find("UI_ClosePanel").gameObject.SetActive(true);
            SaveSlot.GetComponent<Button>().interactable = false;
        }
        else
        {
            SaveSlot.transform.Find("UI_ClosePanel").gameObject.SetActive(false);
            SaveSlot.transform.GetChild(2).gameObject.AddUIEvent(OnSaveSelect, UI_EventHandler.UIEvent.LClick);
            SaveSlot.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = data.playerName;
            SaveSlot.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = data.playCount.ToString();

        }
    }
    private void OnSaveSelect(PointerEventData data)
    {

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
