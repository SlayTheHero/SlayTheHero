using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ���� �ʱ�ȭ
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
