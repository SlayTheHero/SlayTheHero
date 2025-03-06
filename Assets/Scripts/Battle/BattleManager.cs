using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(StateComponent))]
public class BattleManager : MonoBehaviour
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
    public BattleUI BattleUI;
    public UnitSpawner PlayerUnitSpawner;
    public UnitSpawner HeroUnitSpawner;
    public StateComponent StateComponent;
    public Dictionary<int, GameObject> Units;
    public List<PlayerUnit> PlayerTeam;
    public List<HeroUnit> HeroTeam;
    public List<UnitBase> WaitingUnitsList;

    public UnitBase CurUnit;

    public StageData CurStage;

    public float ready_duration;
    public float battle_duration;
    public float end_duration;

    public UnityEvent<int, UnitBase> SkillUsed;
    public UnityEvent<UnitBase> UnitDead;
    public UnityEvent<int> BattleStart;
    public UnityEvent<int, int> SubStageClear;

    private void Awake()
    {
        SkillUsed = new UnityEvent<int, UnitBase>();
        Units = new Dictionary<int, GameObject>();
        BattleStart = new();
        BattleStart.AddListener(Init);
        UnitDead = new();
        SubStageClear = new();
        WaitingUnitsList = new List<UnitBase>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleStart?.Invoke(1); 
        }
    }
    void LoadHeroTeam()
    {
        int pos = 5;
        HeroTeam = CurStage.HeroUnits;
        foreach (var item in HeroTeam)
        {
            WaitingUnitsList.Add(item);
            item.Position = pos++;
            item.Status.HP = 1;
        }
    }
    void LoadPlayerTeam()
    {
        PlayerTeam = new();
        int pos = 1;
        //유닛 선택창에서 선택한 유닛 리스트 가져오기
        foreach (UnitBase unit in PlayerUnitContainer.GetUnitList())
        {
            var p_unit = new PlayerUnit(unit);
            p_unit.IsPlayerUnit = true;
            WaitingUnitsList.Add(p_unit);
            p_unit.Position = pos++;
            PlayerTeam.Add(p_unit);
        }
    }

    public void Init(int stage)
    {
        if (CurStage == null)
            CurStage = StageDB.GetStageData(stage, 1);
        LoadHeroTeam();
        LoadPlayerTeam();
        PlayerUnitSpawner.SpawnAll();
        HeroUnitSpawner.SpawnAll();
        BattleUI.InitWaitingUnitInfo(WaitingUnitsList);
        foreach (var item in WaitingUnitsList)
        {
            item.Status.Waiting = (int)((1f / item.Status.Speed) * 10000);
        }
        StateComponent.AddState(new ReadyBattlePhase(BattlePhaseEnum.ReadyBattlePhase).SetDuration(ready_duration));
        StateComponent.AddState(new BattlePhase(BattlePhaseEnum.BattlePhase).SetDuration(battle_duration));
        StateComponent.AddState(new EndBattlePhase(BattlePhaseEnum.EndBattlePhase).SetDuration(end_duration));
        StateComponent.FSMStart((int)BattlePhaseEnum.ReadyBattlePhase);

    }



    public void UnitSort()
    {
        WaitingUnitsList.Sort(UnitBase.SpeedCompare);
        for (int i = 0; i < WaitingUnitsList.Count; i++)
        {
            WaitingUnitsList[i].Order = i;
        }
        CurUnit = WaitingUnitsList[0];
    }
    public void UnitWaitingDecrease()
    {
        for (int i = 1; i < WaitingUnitsList.Count; i++)
        {
            WaitingUnitsList[i].Status.Waiting -= CurUnit.Status.Waiting;
        }
        CurUnit.Status.Waiting = (int)((1f / CurUnit.Status.Speed) * 10000);
    }

    public void UnitDestroy(UnitBase unit)
    {
        if (CurUnit.Equals(unit))
            CurUnit = null;
        WaitingUnitsList.Remove(unit);
        if (!Units[unit.Position].IsDestroyed())
            Destroy(Units[unit.Position]);
        Units.Remove(unit.Position);
        if (unit.IsPlayerUnit)
        {
            PlayerTeam.RemoveAt(unit.Position - 1);
        }
        else
        {
            HeroTeam.RemoveAt(unit.Position % 4 - 1);
        }

    }
    public void ReorderUnit(bool is_player_team)
    {
        if (is_player_team)
            for (int i = 0; i < PlayerTeam.Count; i++)
            {
                var unit = PlayerTeam[i];
                int pos = unit.Position;
                var uo = Units[pos];
                if (unit.Position > i + 1)
                {
                    int diff = unit.Position - (i + 1);
                    for (int j = 0; j < diff; j++)
                    {
                        uo.GetComponent<UnitObject>().MoveFront();
                        unit.Position--;
                    }
                    Units.Remove(pos);
                    Units.Add(unit.Position, uo);
                }
            }
        else
            for (int i = 0; i < HeroTeam.Count; i++)
            {
                var unit = HeroTeam[i];
                int pos = unit.Position;
                var uo = Units[pos];
                if (unit.Position > i + 5)
                {
                    int diff = unit.Position - (i + 5);
                    for (int j = 0; j < diff; j++)
                    {
                        uo.GetComponent<UnitObject>().MoveFront();
                        unit.Position--;
                    }
                    Units.Remove(pos);
                    Units.Add(unit.Position, uo);
                }
            }

    }

    public void StageClear()
    {
        Debug.Log("CLEAR");
        StateComponent.FSMStop();
    }

    public void StageFail()
    {
        Debug.Log("CLEAR");
        StateComponent.FSMStop();
    }
    public StageData MoveNextStage()
    {
        if ((CurStage = StageDB.GetStageData(CurStage.ID, CurStage.SubStageID + 1)) != null)
        {
            SubStageClear.Invoke(CurStage.ID, CurStage.SubStageID);
            LoadHeroTeam();
            HeroUnitSpawner.SpawnAll();
            foreach (var item in WaitingUnitsList)
            {
                item.Status.Waiting = (int)((1f / item.Status.Speed) * 10000);
            }
            BattleUI.InitWaitingUnitInfo(WaitingUnitsList);
        }
        return CurStage;
    }
    public bool IsStageClear()
    {
        return PlayerTeam.Count == 0 || HeroTeam.Count == 0;
    }
    
}
