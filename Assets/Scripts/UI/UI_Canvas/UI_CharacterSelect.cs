using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
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
        UI_CharacterSelectGirdPanel,
        UI_Skill_1, UI_Skill_2, UI_Skill_3, UI_Skill_4,
    }

    enum Images
    { 
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
        Bind<TextMeshProUGUI>(typeof(Texts));

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
        for (int i = 0; i < 3; i++)
        {
            manager.PlayerData.unitDeque.AddUnit(li[i]);
        }
        manager.PlayerData.playerName = "Wdas";
        manager.PlayerData.playCount = 3;
        SaveManager.SaveData(manager.PlayerData,0);
        manager.PlayerData = SaveManager.LoadData(0);
        SaveManager.SaveData(manager.PlayerData, 1);
        manager.PlayerData.playerName = "asf";
        SaveManager.SaveData(manager.PlayerData, 2);
        SaveManager.SaveFileToClient();

        int unitCount = manager.PlayerData.unitDeque.GetUnitCount();
        GameObject UI_CharacterListGridPanel = GetGameObject((int)GameObjects.UI_CharacterListGridPanel);
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Element/UI_Button");
        for (int i = 0; i < unitCount; i++)
        {
            GameObject temp = GameObject.Instantiate(prefab, UI_CharacterListGridPanel.transform);
            temp.name = "UI_Character_" + i;

            temp.gameObject.AddUIEvent(OnCharacterClicked, UI_EventHandler.UIEvent.LClick);
        }


    }

    private void OnCharacterClicked(PointerEventData data)
    {
        string[] nameArr = data.selectedObject.gameObject.name.Split("_");
        int index = int.Parse(nameArr[2]);
        UnitBase unit = manager.PlayerData.unitDeque.GetUnit(index);

        GetTextMeshPro((int)Texts.UI_Name).text = unit.Name;
        GetTextMeshPro((int)Texts.UI_TraitText).text = unit.Feature.ToString();
        GetTextMeshPro((int)Texts.UI_ClassText).text = unit.Job.ToString();
        GetTextMeshPro((int)Texts.UI_RaceText).text = unit.Race.ToString();

        for (int i = 0; i < 3; i++)
        {
            if(i < unit.SkillList.Count)
            {
                GetGameObject((int)GameObjects.UI_Skill_1 + i).GetComponent<UI_SkillToolTipEventHandler>().setSkillID(unit.SkillList[i].id);
            }
            else
            {

            }
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
