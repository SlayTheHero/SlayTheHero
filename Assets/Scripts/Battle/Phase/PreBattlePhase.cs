using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 배틀 준비를 하는 단계 정렬 -> 가장 빠른 유닛 스테이징
/// </summary>
public class PreBattlePhase : Phase
{

    public PreBattlePhase(Phases _step) : base(_step)
    {
    }

    public override void OnEnterPhase()
    {
        var bm = BattleManager.Instance;
        SortUnit(bm.TurnList);
        bm.StagedUnit = bm.TurnList[0];
        if(bm.StagedUnit.IsPlayerUnit)
            bm.GetGameObject(bm.StagedUnit).transform.GetChild(2).gameObject.SetActive(true);
        else
            bm.GetGameObject(bm.StagedUnit).transform.GetChild(0).gameObject.SetActive(true);

        bm.OnPreBattlePhase.Invoke();
        Debug.Log("Enter PreBattlePhase");
        bm.ChangePhase(Phases.BattlePhase);
        
    }

    public override void OnExitPhase()
    {
        
    }

    void SortUnit(List<UnitBase> turnList)
    {
        turnList.Sort(SpeedCompare);
    }

    int SpeedCompare(UnitBase a, UnitBase b)
    {
        return (int)(a.Status.Waiting - b.Status.Waiting);
    }

}
