using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ReadyBattlePhase : BattlePhaseBase
{
    public ReadyBattlePhase(BattlePhaseEnum name) : base(name) { }

    public override void OnStateEnter()
    {
        var bm = BattleManager.Instance;
        bm.UnitSort();
        m_duration = Duration;
        bm.Units[bm.WaitingUnitsList[0].Position].GetComponent<UnitController>().SetState(UnitController.UnitState.Attacker);
        for (int i = 1; i < bm.WaitingUnitsList.Count; i++)
        {
            bm.Units[bm.WaitingUnitsList[i].Position].GetComponent<UnitController>().SetState(UnitController.UnitState.Default);
        }
    }

    public override void OnStateUpdate()
    {

        m_duration -= Time.deltaTime;

    }

    public override void OnStateExit()
    {
    }
    public override void OnStateInit()
    {

        AddTransition((int)BattlePhaseEnum.BattlePhase, IsDurationExpired);
    }
}
