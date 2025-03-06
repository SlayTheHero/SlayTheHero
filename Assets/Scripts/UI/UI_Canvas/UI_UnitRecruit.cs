using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Synergy;

public class UI_UnitRecruit : UI_Base
{

    private static readonly Dictionary<int, List<SynergyType>> INDEX_SYNERGY_DATA = new Dictionary<int, List<SynergyType>>
    {    
        { 0, new List<SynergyType> { SynergyType.SwordMan, SynergyType.Vampire, SynergyType.SuspiciousGhost, SynergyType.Sloth  } },
        { 1, new List<SynergyType> { SynergyType.Magician, SynergyType.Ghost, SynergyType.Envy } },
        { 2, new List<SynergyType> { SynergyType.Archer, SynergyType.DemonBeast, SynergyType.Swiftness } }
    };

    enum GameObjects
    { 
    }

    enum Images
    { 
    }
    enum Texts
    {
    }
    enum Buttons
    {
        UI_RecruitSlot_1, UI_RecruitSlot_2, UI_RecruitSlot_3, UI_CloseButton
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

        GetButton((int)Buttons.UI_RecruitSlot_1).gameObject.AddUIEvent(OnUnitRecruitSlotClicked, UI_EventHandler.UIEvent.LClick);
        GetButton((int)Buttons.UI_RecruitSlot_2).gameObject.AddUIEvent(OnUnitRecruitSlotClicked, UI_EventHandler.UIEvent.LClick);
        GetButton((int)Buttons.UI_RecruitSlot_3).gameObject.AddUIEvent(OnUnitRecruitSlotClicked, UI_EventHandler.UIEvent.LClick);
        GetButton((int)Buttons.UI_CloseButton).gameObject.AddUIEvent(p => { manager.UI.ClosePopupUI(); }, UI_EventHandler.UIEvent.LClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    } 
    // 캐릭터 생성 어케할건데?
    void OnUnitRecruitSlotClicked(PointerEventData data)
    {
        string[] nameArr = data.selectedObject.gameObject.name.Split("_");
        int index = int.Parse(nameArr[2]);
        GetNewUnit(index);
        manager.UI.ClosePopupUI();
        manager.UI.ShowPopupUI<UI_UnitRecruit_Hunting>();
    }
    

    void GetNewUnit(int index)
    {
        List<SynergyType> list = INDEX_SYNERGY_DATA[index];
        int unitIndex = UnityEngine.Random.Range(0, list.Count);
        UnitBase unit = UnitDB.GetUnitForSynergy(list[unitIndex]);
        manager.PlayerData.unitDeque.AddUnit(unit);
    }

}
     
