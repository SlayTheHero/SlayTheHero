using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_CharacterSelect : UI_Base
{
    enum GameObjects
    {
        UI_CharacterListPanel,
        UI_CharaterListScrollBar,
        UI_CharacterListGridPanel,
        UI_CharacterDialoguePanel,
        UI_SkillGridPanel,
        UI_CharacterSelectGirdPanel,
        UI_Skill_1, UI_Skill_2, UI_Skill_3, UI_Skill_4,
        UI_RaceIcon, UI_ClassIcon, UI_TraitIcon,
        UI_SynergyDisplay
    }

    enum Images
    { 
        UI_RaceIcon, UI_ClassIcon, UI_TraitIcon, UI_Portrait,
        UI_Skill_1, UI_Skill_2, UI_Skill_3, UI_Skill_4, 
        UI_CharacterSlot_1, UI_CharacterSlot_2, UI_CharacterSlot_3,
        UI_Synergy_1, UI_Synergy_2, UI_Synergy_3,
        UI_StartButton,UI_SettingButton
    }
    enum Texts
    {
        UI_RaceText, UI_ClassText, UI_TraitText, UI_Name
    }
    enum Buttons
    {
        UI_StartButton, UI_SettingButton, UI_CloseButton,
        UI_CharacterSlot_1, UI_CharacterSlot_2, UI_CharacterSlot_3
    }

    GameManager manager;
    UI_CharacterListPanel characterList;
    protected override void Init()
    { 
        manager = GameManager.getInstance();
        manager.UI.SetCanvas(this.gameObject, true);
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        setDeck(IsDeck);
        if(manager.PlayerData.unitDeque.GetUnitCount() == 0)
        { 
            for (int i = 3; i < 6; i++)
            {
                manager.PlayerData.unitDeque.AddUnit(UnitDB.GetUnit(i));
            }
            for (int i = 3; i < 6; i++)
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
            SaveManager.SaveData(manager.PlayerData, 0);
            manager.PlayerData = SaveManager.LoadData(0);
            SaveManager.SaveData(manager.PlayerData, 1);
            manager.PlayerData.playerName = "asf";
            SaveManager.SaveData(manager.PlayerData, 2);
            SaveManager.SaveFileToClient();
        }else
        {
            SaveManager.LoadData(0);
        }

        int unitCount = manager.PlayerData.unitDeque.GetUnitCount();
        GameObject UI_CharacterListPanel = GetGameObject((int)GameObjects.UI_CharacterListPanel);
        characterList = UI_CharacterListPanel.GetComponent<UI_CharacterListPanel>();
        characterList.LoadPlayerData();
        characterList.SetUnitEvent(OnCharacterClicked, UI_EventHandler.UIEvent.LClick);

        selectedArr = new int[3]; Array.Fill(selectedArr, -1);
        nowSynergy = new (int, bool)[3]; Array.Fill(nowSynergy, (-1,false));


        GetButton((int)Buttons.UI_StartButton).gameObject.AddUIEvent(OnStartButtonClicked, UI_EventHandler.UIEvent.LClick);            GetImage((int)Images.UI_CharacterSlot_1).gameObject.AddUIEvent(OnSlotClicked, UI_EventHandler.UIEvent.LClick);
 
        GetImage((int)Images.UI_CharacterSlot_1).gameObject.AddUIEvent(OnSlotClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_CharacterSlot_2).gameObject.AddUIEvent(OnSlotClicked, UI_EventHandler.UIEvent.LClick);
        GetImage((int)Images.UI_CharacterSlot_3).gameObject.AddUIEvent(OnSlotClicked, UI_EventHandler.UIEvent.LClick);

        GetButton((int)Buttons.UI_CloseButton).gameObject.AddUIEvent(OnCloseButtonClicked, UI_EventHandler.UIEvent.LClick);

        isStartReady = false;
        setStartButton();
        setSynergyImage();
    }

    int[] selectedArr;
    // true면 3개
    (int,bool)[] nowSynergy;
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
                GetGameObject((int)GameObjects.UI_Skill_1 + i).GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Skill, unit.SkillList[i].id);
            }
            else
            {
                GetGameObject((int)GameObjects.UI_Skill_1 + i).GetComponent<UI_SkillToolTipEventHandler>().setSkillID(-1);
                GetGameObject((int)GameObjects.UI_Skill_1 + i).GetComponent<Image>().sprite = null;
            }
        }
        GetImage((int)Images.UI_RaceIcon).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)unit.Race);
        GetImage((int)Images.UI_ClassIcon).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)unit.Job + 4);
        GetImage((int)Images.UI_TraitIcon).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)unit.Feature + 7);
        GetGameObject((int)GameObjects.UI_RaceIcon).GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID((int)unit.Race);
        GetGameObject((int)GameObjects.UI_ClassIcon).GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID((int)unit.Job + 4);
        GetGameObject((int)GameObjects.UI_TraitIcon).GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID((int)unit.Feature + 7);

        GameObject target = null; 
        // 다시 클릭시 빠지는 로직.
        if (index == selectedArr[0])
        {
            target = GetImage((int)Images.UI_CharacterSlot_1).gameObject;
        }
        else if (index == selectedArr[1])
        {
            target = GetImage((int)Images.UI_CharacterSlot_2).gameObject;
         }
        else if (index == selectedArr[2])
        {
            target = GetImage((int)Images.UI_CharacterSlot_3).gameObject;
         }
        if(target != null)
        {
            PointerEventData pData = new PointerEventData(EventSystem.current);
            pData.selectedObject = target;
            ExecuteEvents.Execute(target, pData, ExecuteEvents.pointerClickHandler);
            characterList.SetUnitSelected(index, false);
        }
        else
        { 
            // 비어있는 슬롯에 넣는 로직.
            if (selectedArr[0] == -1)
            {
                GetImage((int)Images.UI_CharacterSlot_1).sprite = data.selectedObject.gameObject.GetComponent<Image>().sprite;
                 selectedArr[0] = index; 
                 characterList.SetUnitSelected(index, true);
            }
            else if (selectedArr[1] == -1)
            {
                GetImage((int)Images.UI_CharacterSlot_2).sprite = data.selectedObject.gameObject.GetComponent<Image>().sprite;
                 selectedArr[1] = index;
                characterList.SetUnitSelected(index, true);
            }
            else if (selectedArr[2] == -1)
            {
                GetImage((int)Images.UI_CharacterSlot_3).sprite = data.selectedObject.gameObject.GetComponent<Image>().sprite;
                 selectedArr[2] = index;
                characterList.SetUnitSelected(index, true); 
            } 
        }

        setStartButton();
        setSynergyImage();
    }

    private void OnSlotClicked(PointerEventData data)
    {
        int index = int.Parse(data.selectedObject.name.Split("_")[2]) - 1;
        if (selectedArr[index] != -1)
        {
            switch(index)
            {
                case 0:
                    GetImage((int)Images.UI_CharacterSlot_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Default,0);
                    break;
                case 1:
                    GetImage((int)Images.UI_CharacterSlot_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
                    break;
                case 2:
                    GetImage((int)Images.UI_CharacterSlot_3).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
                    break;
            }
            characterList.SetUnitSelected(selectedArr[index], false);
            selectedArr[index] = -1;
        } 
        setStartButton();
        setSynergyImage();
    }

    
    private void setSynergyImage()
    {
        List<(int,bool)> nowSynergy = new List<(int,bool)>();
        List<UnitBase> unit = new List<UnitBase>();
        for (int i = 0; i < 3; i++)
        {
            bool select = selectedArr[i] == -1;
            if(!select)
            {
                unit.Add(manager.PlayerData.unitDeque.GetUnit(selectedArr[i]));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            GetImage((int)Images.UI_Synergy_1).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetImage((int)Images.UI_Synergy_2).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            GetImage((int)Images.UI_Synergy_3).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
        }

        nowSynergy = SynergyDB.getSynergyFromUnitList(unit); 
        GetGameObject((int)GameObjects.UI_SynergyDisplay).GetComponent<UI_SynergyDisplay>().InitializeDisplay(unit);

        if (nowSynergy == null)
        {
            return;
        }
        for (int i = 0; i < nowSynergy.Count; i++)
        {
            GetImage((int)Images.UI_Synergy_1 + i).sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, nowSynergy[i].Item1);
        }
         

    }
    private void setStartButton()
    {
        if (selectedArr[0] != -1 && selectedArr[1] != -1 && selectedArr[2] != -1)
        {
            isStartReady = true;
            GetButton((int)Buttons.UI_StartButton).interactable = true;
        }
        else
        {
            isStartReady = false;
            GetButton((int)Buttons.UI_StartButton).interactable = false;
        }
    }
    bool isStartReady = false; 
    public void tempEvent(PointerEventData data)
    {
        data.pointerClick.GetComponent<Image>().color = Color.red;
    }

    public void OnStartButtonClicked(PointerEventData data)
    {
        if (isStartReady)
        {
            SceneController.ChangeScene(SceneController.SceneType.Maintenance);
        }
    }

    public void OnCloseButtonClicked(PointerEventData data)
    {
        if(IsDeck)
        {
            manager.UI.ClosePopupUI();
        }

    }
    bool IsDeck = false;
    public void setDeck(bool isDeck)
    {
        IsDeck = isDeck;
        if (isDeck)
        {
            GetButton((int)Buttons.UI_CloseButton).gameObject.SetActive(true);
            GetButton((int)Buttons.UI_SettingButton).gameObject.SetActive(false);
            GetButton((int)Buttons.UI_StartButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.UI_CloseButton).gameObject.SetActive(false);
            GetButton((int)Buttons.UI_SettingButton).gameObject.SetActive(true);
            GetButton((int)Buttons.UI_StartButton).gameObject.SetActive(true);
        }
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
