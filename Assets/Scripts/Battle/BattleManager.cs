using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public partial class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("BattleManager")?.GetComponent<BattleManager>();
            return instance;
        }
    }
    [SerializeField]
    GameObject[] PlayerTeamPosition = new GameObject[4];
    [SerializeField]
    GameObject[] HeroTeamPosition = new GameObject[4];

    public int CurStage = 1;
    public int CurSubStage = 1;
    public List<PlayerUnit> PlayerTeam;
    public List<HeroUnit> HeroTeam;
    public List<UnitBase> TurnList;
    public UnitBase StagedUnit;

    public UnityEvent OnPreBattlePhase;
    public UnityEvent OnBattlePhase;
    public UnityEvent OnPostBattlePhase;

    public UnityEvent OnSkillUsed;

    public UnityEvent OnTurnEnd;

    public UnityEvent OnUnitDead;

    public Phase CurPhase;

    public int? selected_skill;

    private void Awake()
    {
        if (instance == null)
        {
            Init();
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Update()
    {

        if (selected_skill != null && Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider)
            {
                SelectTarget(int.Parse(hit.collider.gameObject.name), hit.collider.gameObject.GetComponentInParent<Transform>().gameObject.name == "PlayerPos");
            }
            else
            {
                selected_skill = null;
            }
        }
    }
    void Init()
    {
        CurStage = 1;
        CurSubStage = 1;
        TurnList = new List<UnitBase>();
        OnSkillUsed.AddListener(() => ChangePhase(Phases.PostBattlePhase));
        LoadHeroTeam();
        LoadPlayerTeam();
        InitWaiting();
        PreBattlePhase p = new(Phases.PreBattlePhase);
        BattlePhase b = new(Phases.BattlePhase);
        PostBattlePhase post = new(Phases.PostBattlePhase);

        CurPhase = p;
        CurPhase.OnEnterPhase();

    }

    public void ChangePhase(Phases next)
    {
        CurPhase.OnExitPhase();
        CurPhase = Phase.PhaseTable[next];
        CurPhase.OnEnterPhase();
    }

    void LoadHeroTeam()
    {
        HeroTeam = new List<HeroUnit>();

        var data = StageDB.GetStageData(CurStage, CurSubStage);
        int i = 1;
        foreach (var item in data.HeroUnits)
        {
            HeroTeam.Add(item);
            TurnList.Add(item);
            HeroTeamPosition[i-1].SetActive(true);
            HeroTeam.Last().Position = i++;
        }
        
    }
    void LoadPlayerTeam()
    {
        PlayerTeam = new();
        var list = PlayerUnitContainer.GetUnitList();
        for (int i = 0; i < list.Count; i++)
        {
            PlayerTeam.Add(new PlayerUnit(list[i]));
            TurnList.Add(PlayerTeam[i]);
            PlayerTeam[i].Position = i + 1;
            PlayerTeamPosition[i].SetActive(true);
        }

    }

    public void UseSkill(int id)
    {
        selected_skill = id;
    }
    public void SelectTarget(int target, bool isPlayerUnit)
    {
        if (selected_skill == null)
            return;
        if (isPlayerUnit)
        {
            if (PlayerTeam.Count > target)
                StagedUnit.SkillList[selected_skill.Value].Invoke(StagedUnit, PlayerTeam[target - 1]);
        }
        else
        {
            if (HeroTeam.Count > target)
                StagedUnit.SkillList[selected_skill.Value].Invoke(StagedUnit, HeroTeam[target - 1]);
        }


    }

    private void InitWaiting()
    {
        foreach (UnitBase a in TurnList)
        {
            a.Status.Waiting = (int)((1f / a.Status.Speed) * 10000);
        }
    }

    void OnUnitDeadHandler(UnitBase unit)
    {
        var pos = unit.Position;
        var is_player = unit.IsPlayerUnit;
        TurnList.Remove(unit);
        if (is_player)
        {
            var p_unit = unit as PlayerUnit;
            for (int i = pos - 1; i < PlayerTeam.Count; i++)
            {
                PlayerTeam[i].Position--;
            }
            PlayerTeam.Remove(p_unit);
            PlayerTeamPosition.Last().SetActive(false);
        }
        else
        {
            var h_unit = unit as HeroUnit;

            for (int i = pos - 1; i < HeroTeam.Count; i++)
            {
                HeroTeam[i].Position--;
            }

            HeroTeam.Remove(h_unit);

            HeroTeamPosition.Last().SetActive(false);
        }
    }
}

