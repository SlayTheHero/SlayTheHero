using fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : BattlePhaseBase
{
    bool is_playing = false;
    public BattlePhase(BattlePhaseEnum phase_enum) : base(phase_enum)
    {
    }
    public override void OnStateEnter()
    {
        is_playing = false;
        m_duration = 0;
        var bm = BattleManager.Instance;
        var unit = bm.CurUnit;
        if (!unit.IsPlayerUnit)
        {
            m_duration = Duration;
            (unit as HeroUnit).Behave();
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

    }
    public override void OnStateInit()
    {
        BattleManager.Instance.TurnSkip.AddListener(() =>
        {
            if (!is_playing)
                m_duration = -1;
        });
        BattleManager.Instance.SkillUsed.AddListener((target) =>
        {
            is_playing = true;
            BattleManager.Instance.CurUnit.SkillList[BattleManager.Instance.SelectedSkillNum].Invoke(BattleManager.Instance.CurUnit, target
                );
            m_duration = Duration;
        });
        AddTransition((int)BattlePhaseEnum.EndBattlePhase, IsDurationExpired);
    }
}
