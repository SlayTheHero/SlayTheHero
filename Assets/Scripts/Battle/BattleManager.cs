using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
using UnityEngine.UI;

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
    public Image Bgd;
    [SerializeField]
    public GameObject[] PlayerTeamPosition = new GameObject[4];
    [SerializeField]
    public GameObject[] HeroTeamPosition = new GameObject[4];

    public bool[] player_range = new bool[4];
    public bool[] hero_range = new bool[4];

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

    public UnityEvent OnUnitInit;

    int unit_init = 0;

    public Phase CurPhase;
    
    public int selected_skill = -1;

    public GameObject skill_target;
    

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
    private void Start()
    {
        for (int i = 0; i < HeroTeam.Count; i++)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Units/" + HeroTeam[i].Name), HeroTeamPosition[i].transform);
        }
    }
    private void Update()
    {

        if (selected_skill > -1 && Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            var target = hit ? hit.collider.gameObject : null;
            if (target == null)
            {
                AlphaReset();
                selected_skill = -1;
                return;
            }
            Debug.Log(target);
        }
    }
    void Init()
    {
        CurStage = 1;
        CurSubStage = 1;
        TurnList = new List<UnitBase>();
        OnSkillUsed.AddListener(() => ChangePhase(Phases.PostBattlePhase));
        OnUnitInit.AddListener(() => { unit_init++; if (unit_init == TurnList.Count) CurPhase.OnEnterPhase(); });
        LoadHeroTeam();
        LoadPlayerTeam();
        InitWaiting();
        PreBattlePhase p = new(Phases.PreBattlePhase);
        BattlePhase b = new(Phases.BattlePhase);
        PostBattlePhase post = new(Phases.PostBattlePhase);

        CurPhase = p;
        

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
            HeroTeamPosition[i - 1].SetActive(true);
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
        if (!(StagedUnit.IsPlayerUnit))
            return;
        selected_skill = id;
        Bgd.color = new Color(0, 0, 0, 0.8f);
        var range = StagedUnit.SkillList[id].range;
        var pos = StagedUnit.Position;
        var back = pos + range > 4 ? 4 : pos + range;
        var front = pos - range;
        foreach (var item in PlayerTeam)
        {
            var ipos = item.Position;
            if (ipos <= back && ipos >= front)
            {
                ChageAlpha(PlayerTeamPosition[ipos - 1].transform.GetChild(1).gameObject, true);
                PlayerTeamPosition[ipos - 1].GetComponent<BoxCollider2D>().enabled = true;
                player_range[ipos - 1] = true;
            }
            else
            {
                ChageAlpha(
                PlayerTeamPosition[ipos - 1].transform.GetChild(1).gameObject, false);
                PlayerTeamPosition[ipos - 1].GetComponent<BoxCollider2D>().enabled = false;
                player_range[ipos - 1] = false;
            }
        }
        foreach (var item in HeroTeam)
        {
            var ipos = item.Position;
            var h_back = (-1 * front) + 1;

            if (ipos > h_back)
            {

                ChageAlpha(HeroTeamPosition[ipos - 1], false);

                HeroTeamPosition[ipos - 1].GetComponent<BoxCollider2D>().enabled = false;
                hero_range[ipos - 1] = false;
                continue;
            }
            ChageAlpha(HeroTeamPosition[ipos - 1], true);
            HeroTeamPosition[ipos - 1].GetComponent<BoxCollider2D>().enabled = true;
            hero_range[ipos - 1] = true;

        }

    }
    public void SelectTarget(int target, bool isPlayerUnit)
    {
        if (selected_skill == -1)
            return;
        if (isPlayerUnit)
        {
            if (PlayerTeam.Count > target)
            {
                skill_target = PlayerTeamPosition[target - 1];
                StagedUnit.SkillList[selected_skill].Invoke(StagedUnit, PlayerTeam[target - 1]);
            }
        }
        else
        {
            if (HeroTeam.Count > target)
            {
                skill_target = HeroTeamPosition[target - 1];
                StagedUnit.SkillList[selected_skill].Invoke(StagedUnit, HeroTeam[target - 1]);
            }
        }
        AlphaReset();
        selected_skill = -1;
        Bgd.color = new Color(0, 0, 0, 0);
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
    public void ChageAlpha(GameObject target, bool can_select)
    {
        var sr = target.GetComponentsInChildren<SpriteRenderer>();
        foreach (var c in sr)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, can_select ? 1f : 0.5f);
        }
    }
    public void AlphaReset()
    {
        foreach (var u in PlayerTeamPosition)
        {
            ChageAlpha(u, true);
            u.GetComponent<BoxCollider2D>().enabled = true;
        }
        foreach (var u in HeroTeamPosition)
        {
            ChageAlpha(u, true);
            u.GetComponent<BoxCollider2D>().enabled = true;
        }
        for (int i = 0; i < 4; i++)
        {
            player_range[i] = true;
            hero_range[i] = true;
        }
        Bgd.color = new Color(0, 0, 0, 0);
    }
    public GameObject GetGameObject(UnitBase unit)
    {
        if (unit.IsPlayerUnit)
            return PlayerTeamPosition[unit.Position-1];
        return HeroTeamPosition[unit.Position-1];
    }
}

