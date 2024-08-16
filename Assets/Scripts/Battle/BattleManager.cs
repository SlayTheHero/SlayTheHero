using System;
using System.Collections;
using System.Collections.Generic;
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

    bool is_skill_used = false;
    int skillIndex = -1;
    UI_BattleScene ui_BattleScene;

    public int CurStage = 0;
    public List<PlayerUnit> PlayerTeam;
    public List<HeroUnit> HeroTeam;
    public List<UnitBase> TurnList;
    public UnitBase StagedUnit;
    public Action OnSkillUsed;

    public UnityEvent OnTurnEnd;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadHeroTeam();
        SetUnitOnScene();
        InitWaiting();
        PrepareNewTurn();
    }
    private void Update()
    {

        if (is_skill_used && Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider)
            {
                SelectTarget(int.Parse(hit.collider.gameObject.name));
            }
            else
            {
                is_skill_used = false;
                skillIndex = -1;
            }
        }
    }

    void LoadHeroTeam()
    {
        if (HeroTeam == null)
            HeroTeam = new List<HeroUnit>();
        else
            HeroTeam.Clear();
        var data = StageDB.GetStageData(CurStage);
        foreach (var item in data.HeroUnits)
        {
            HeroTeam.Add(item);
        }
    }

    void SortUnitOrder()
    {

        TurnList.Sort(delegate (UnitBase a, UnitBase b) { return UnitSpeedCompare(a, b); });

    }
    void ChangeStagedUnit()
    {

        StagedUnit = TurnList[0];
        foreach (UnitBase a in TurnList)
        {
            a.Status.Waiting -= StagedUnit.Status.Waiting > 0 ? StagedUnit.Status.Waiting : 0;
        }

    }
    void PrepareNewTurn()
    {

        SortUnitOrder();
        ChangeStagedUnit();
        if (!StagedUnit.IsPlayerUnit)
        {
            var h = StagedUnit as HeroUnit;
            h.Behave();
        }
    }
    public void UseSkill(int index)
    {
        is_skill_used = true;
        skillIndex = index;
        Debug.Log("UseSkill");
    }
    public void SelectTarget(int index)
    {
        StagedUnit.SkillList[skillIndex].Invoke(StagedUnit, StagedUnit.IsPlayerUnit ?
            HeroTeamPosition[index].GetComponentInChildren<HeroUnit>() : PlayerTeamPosition[index].GetComponentInChildren<PlayerUnit>());
        Debug.Log("Select Target");
    }
    public int UnitSpeedCompare(UnitBase a, UnitBase b)
    {
        return (int)(a.Status.Waiting - b.Status.Waiting);
    }
    private void InitWaiting()
    {
        foreach (UnitBase a in TurnList)
        {
            a.Status.Waiting = 1 / a.Status.Speed;
        }
    }
    void SetUnitOnScene()
    {
        var h = Resources.Load<GameObject>("Prefabs/HeroUnit");
        var p = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        for (int i = 0; i < HeroTeam.Count; i++)
        {
            Instantiate(h, HeroTeamPosition[i].transform);
        }
        for (int i = 0; i < PlayerTeam.Count; i++)
        {
            Instantiate(p, PlayerTeamPosition[i].transform);
        }

    }
    void OnSkillUsedHandler()
    {
        PrepareNewTurn();
    }
}

