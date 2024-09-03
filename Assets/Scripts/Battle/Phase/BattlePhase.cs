using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 관련 변수 초기화
/// </summary>

public class BattlePhase : Phase
{
    public BattlePhase(Phases _step) : base(_step)
    {
    }
    public override void OnEnterPhase()
    {

        Debug.Log("Enter BattlePhase");
        var bm = BattleManager.Instance;
        bm.selected_skill = null;
        if (!bm.StagedUnit.IsPlayerUnit)
        {
            HeroUnit h = bm.StagedUnit as HeroUnit;
            h.Behave();
        }
    }
    public override void OnExitPhase()
    {
    }
}
