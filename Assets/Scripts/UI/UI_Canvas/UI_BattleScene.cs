using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattleScene : UI_Base
{
    enum GameObjects
    {
        BattleInfoPanel,
        TurnGridPanel,
        BattleControlPanel,
        UI_SynergyDisplay,
    }
    enum Images
    {
        Portrait,
        BattleSceneBgdImage,
        UI_SpecialSkill,
    }
    enum Buttons
    {
        SettingButton,
        Confirm,
        SkipButton,

    }

    enum Skills
    {
        UI_BaseAttack,
        UI_Skill_1,
        UI_Skill_2,
        UI_Skill_3,
    }
    enum Turns
    {
        UI_Turn_1,
        UI_Turn_2,
        UI_Turn_3,
        UI_Turn_4,
        UI_Turn_5,
        UI_Turn_6,
        UI_Turn_7,
        UI_Turn_8,

    }

    protected override void Init()
    {
        // GameManager.UI.SetCanvas(this.gameObject, true);
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<UI_Skill>(typeof(Skills));
        Bind<UI_Turn>(typeof(Turns));

        BattleManager.Instance.StageClear.AddListener(() => GameManager.getInstance().UI.ShowPopupUI<UI_StageEndPopup>("StageEndPopup"));

        GetUI<Image>((int)Images.BattleSceneBgdImage).sprite = ImageDB.GetImage(ImageDB.ImageType.Default, BattleManager.Instance.CurStage.ID);
        GetUI<Image>((int)Images.BattleSceneBgdImage).gameObject.AddUIEvent((p) => OnSceneBgdClicked());
        BattleControlPanel_Init();
        BattleInfoPanel_Init();
        GetUI<GameObject>((int)GameObjects.UI_SynergyDisplay).GetComponent<UI_SynergyDisplay>().InitializeDisplay(BattleManager.Instance.PlayerTeam);
    }

    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.InitDone.AddListener(Init);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region BattleControlPanel

    void BattleControlPanel_Init()
    {
        //스페셜 스킬 초기화
        GetUI<UI_Skill>((int)Skills.UI_BaseAttack).SetInfo((int)Skills.UI_BaseAttack);
        GetUI<UI_Skill>((int)Skills.UI_Skill_1).SetInfo((int)Skills.UI_Skill_1);
        GetUI<UI_Skill>((int)Skills.UI_Skill_2).SetInfo((int)Skills.UI_Skill_2);
        GetUI<UI_Skill>((int)Skills.UI_Skill_3).SetInfo((int)Skills.UI_Skill_3);

        GetUI<UI_Skill>((int)Skills.UI_BaseAttack).gameObject.GetComponent<Button>().onClick.AddListener(() => OnSkillClicked((int)Skills.UI_BaseAttack));
        GetUI<UI_Skill>((int)Skills.UI_Skill_1).gameObject.GetComponent<Button>().onClick.AddListener(() => OnSkillClicked((int)Skills.UI_Skill_1));
        GetUI<UI_Skill>((int)Skills.UI_Skill_2).gameObject.GetComponent<Button>().onClick.AddListener(() => OnSkillClicked((int)Skills.UI_Skill_2));
        GetUI<UI_Skill>((int)Skills.UI_Skill_3).gameObject.GetComponent<Button>().onClick.AddListener(() => OnSkillClicked((int)Skills.UI_Skill_3));

        GetUI<Button>((int)Buttons.SkipButton).gameObject.AddUIEvent((p) => OnSkipButtonClicked());
        SetSkipButtonEnable(false);
        BattleManager.Instance.StateComponent.StateChanged.AddListener((prev, next) =>
        {
            if (next == (int)BattlePhaseEnum.ReadyBattlePhase)
            {
                BattleControlPanel_RefreshUI();
                if (BattleManager.Instance.CurUnit.IsPlayerUnit)
                {
                    SetSkipButtonEnable(true);
                    SetSkillBtnEnable(true);
                }
            }
            else if (next == (int)BattlePhaseEnum.EndBattlePhase)
            {
                SetSkipButtonEnable(false);
            }
        });
        BattleManager.Instance.SkillUsed.AddListener((n) => { SetSkillBtnEnable(false); });
    }
    void BattleControlPanel_RefreshUI()
    {
        GetImage((int)Images.Portrait).sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, BattleManager.Instance.CurUnit.ID);
        GetUI<UI_Skill>((int)Skills.UI_BaseAttack).RefreshUI();
        GetUI<UI_Skill>((int)Skills.UI_Skill_1).RefreshUI();
        GetUI<UI_Skill>((int)Skills.UI_Skill_2).RefreshUI();
        GetUI<UI_Skill>((int)Skills.UI_Skill_3).RefreshUI();
    }
    void SetSkillBtnEnable(bool enable)
    {
        GetUI<UI_Skill>((int)Skills.UI_BaseAttack).gameObject.GetComponent<Button>().interactable = enable;
        GetUI<UI_Skill>((int)Skills.UI_Skill_1).gameObject.GetComponent<Button>().interactable = enable;
        GetUI<UI_Skill>((int)Skills.UI_Skill_2).gameObject.GetComponent<Button>().interactable = enable;
        GetUI<UI_Skill>((int)Skills.UI_Skill_3).gameObject.GetComponent<Button>().interactable = enable;
    }
    void SetSkipButtonEnable(bool enable)
    {
        var skip_btn = GetUI<Button>((int)Buttons.SkipButton).GetComponent<RectTransform>();

        GetUI<Button>((int)Buttons.SkipButton).interactable = enable;
        if (enable)
        {
            skip_btn.DOAnchorPosY(-210f, 0.5f);
        }
        else
        {
            skip_btn.DOAnchorPosY(-500, 0.5f);
        }
    }
    void OnSkillClicked(int skill_idx)
    {
        if (BattleManager.Instance.CurUnit == null)
            return;
        BattleManager.Instance.SelectedSkillNum = skill_idx;
        var unit = BattleManager.Instance.CurUnit;
        var range = unit.SkillList[skill_idx].range;

        int[] arr = { 4, 3, 2, 1, 5, 6, 7, 8 };
        int curidx = Array.IndexOf(arr, unit.Position);
        int left = Mathf.Max(0, curidx - range);
        int right = Mathf.Min(curidx + range, 7);
        for (int i = 0; i < left; i++)
        {
            GameObject untargetable;
            if (BattleManager.Instance.Units.TryGetValue(arr[i], out untargetable))
            {
                untargetable.GetComponent<UnitController>().SetState(UnitController.UnitState.Untargetable);
            }
        }
        for (int i = left; i <= right; i++)
        {
            GameObject targetable;
            if (BattleManager.Instance.Units.TryGetValue(arr[i], out targetable))
            {
                targetable.GetComponent<UnitController>().SetState(UnitController.UnitState.Targetable);
            }
        }
        for (int i = right + 1; i < 8; i++)
        {
            GameObject untargetable;
            if (BattleManager.Instance.Units.TryGetValue(arr[i], out untargetable))
            {
                untargetable.GetComponent<UnitController>().SetState(UnitController.UnitState.Untargetable);
            }
        }



    }
    void OnSceneBgdClicked()
    {
        BattleManager.Instance.SelectedSkillNum = 0;
        foreach (var unit in BattleManager.Instance.Units)
        {
            if (unit.Key == BattleManager.Instance.CurUnit.Position)
            {
                unit.Value.GetComponent<UnitController>().SetState(UnitController.UnitState.Attacker);
                continue;
            }
            unit.Value.GetComponent<UnitController>().SetState(UnitController.UnitState.Default);
        }
    }

    void OnSkipButtonClicked()
    {
        BattleManager.Instance.TurnSkip.Invoke();
        OnSceneBgdClicked();
    }
    #endregion
    #region BattleInfoPanel
    List<UI_Turn> _enable_turn_ui_list;
    List<UI_Turn> _disable_turn_ui_list;
    void BattleInfoPanel_Init()
    {
        _enable_turn_ui_list = new List<UI_Turn>();
        _disable_turn_ui_list = new List<UI_Turn>();

        for (int i = 0; i < BattleManager.Instance.WaitingUnitsList.Count; i++)
        {
            var unit = BattleManager.Instance.WaitingUnitsList[i];
            _enable_turn_ui_list.Add(GetUI<UI_Turn>(i).SetInfo(unit.Position));
        }
        for (int i = BattleManager.Instance.WaitingUnitsList.Count; i < 8; i++)
        {
            GetUI<UI_Turn>(i).gameObject.SetActive(false);
            _disable_turn_ui_list.Add(GetUI<UI_Turn>(i));
        }
        BattleManager.Instance.UnitPositionChanged.AddListener(OnUnitPositionChanged);
        BattleManager.Instance.UnitDead.AddListener(OnUnitDead);
        BattleManager.Instance.UnitSorted.AddListener(OnUnitSorted);
        BattleManager.Instance.SubStageClear.AddListener((a,b)=> OnUnitSpawned());
    }
    void BattleInfoPanel_RefreshUI()
    {
        for (int i = 0; i < 8; i++)
        {
            GetUI<UI_Turn>(i).RefreshUI();
        }
    }
    void OnUnitDead(UnitBase dead_unit)
    {
        var dead_turn_ui = _enable_turn_ui_list.Find((unit) => unit.Position == dead_unit.Position);
        _enable_turn_ui_list.Remove(dead_turn_ui);
        _disable_turn_ui_list.Add(dead_turn_ui);
        dead_turn_ui.DisapearAndDisable();
    }
    void OnUnitPositionChanged(int prev, int next)
    {
        var changed_turn_ui = _enable_turn_ui_list.Find((unit) => unit.Position == prev);
        changed_turn_ui.SetInfo(next);

    }
    void OnUnitSorted()
    {
        for (int i = 0; i < _enable_turn_ui_list.Count; i++)
        {
            var order = BattleManager.Instance.WaitingUnitsList.FindIndex((unit) => unit.Position == _enable_turn_ui_list[i].Position);
            _enable_turn_ui_list[i].OrderChange(order);
        }
    }
    void OnUnitSpawned()
    {
        var list = BattleManager.Instance.HeroTeam;
        for (int i = 0; i < list.Count; i++)
        {
            var temp = _disable_turn_ui_list[0];
            _disable_turn_ui_list.Remove(temp);
            
            temp.SetInfo(list[i].Position);
            temp.AppearAndEnable();
            _enable_turn_ui_list.Add(temp);
        }
        
    }
    #endregion
}
