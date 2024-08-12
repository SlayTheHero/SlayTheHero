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
    GameObject[] PlayerTeamPosition = new GameObject[3];
    [SerializeField]
    GameObject[] HeroTeamPosition = new GameObject[3];

    bool is_skill_used = false;
    int skillIndex = -1;

    public List<PlayerUnit> PlayerTeam;
    public List<HeroUnit> HeroTeam;
    public List<UnitBase> TurnList;
    public UnitBase StagedUnit;
    UI_BattleScene ui_BattleScene;

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
}

