using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ReadyBattlePhase : BattlePhaseBase
{
    public ReadyBattlePhase(BattlePhaseEnum name) : base(name) { }

    public override void OnStateEnter()
    {
        var bm = BattleManager.Instance;
        var ui = bm.BattleUI;
        
        ui.SortWaitingUI(BattleManager.Instance.WaitingUnitsList);
        bm.UnitSort();
        var unit = bm.CurUnit;
        ui.SkillBtnOn(unit.SkillList.Count, false);
        ui.SetCurUnitInfo(unit);
        ui.SetCursorEnable(true,unit.Position);
        m_duration = Duration;

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
