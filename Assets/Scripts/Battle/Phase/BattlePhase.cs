using fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : BattlePhaseBase
{
    public BattlePhase(BattlePhaseEnum phase_enum) : base(phase_enum)
    {
    }
    public override void OnStateEnter()
    {
        m_duration = 0;
        var bm = BattleManager.Instance;
        var ui = bm.BattleUI;
        var unit = bm.CurUnit;
        ui.SkillBtnOn(unit.SkillList.Count, unit.IsPlayerUnit);
        if (!unit.IsPlayerUnit)
        {
            m_duration = Duration;
            ui.SetTargetCursorEnable(true, (unit as HeroUnit).Behave());
        }
    }

    public override void OnStateUpdate()
    {
        if (m_duration > 0)
        {
            m_duration -= Time.deltaTime;
        }
    }

    public override void OnStateExit()
    {
        var ui = BattleManager.Instance.BattleUI;
        ui.SetCursorEnable(false,BattleManager.Instance.CurUnit.Position);
        ui.SetTargetCursorEnable(false, BattleManager.Instance.CurUnit.Position);
    }
    public override void OnStateInit()
    {

        BattleManager.Instance.SkillUsed.AddListener((skill_num, target) =>
        {
            BattleManager.Instance.CurUnit.SkillList[skill_num].Invoke(BattleManager.Instance.CurUnit,target
                );
            m_duration = Duration;
        });
        AddTransition((int)BattlePhaseEnum.EndBattlePhase, IsDurationExpired);
    }
}
