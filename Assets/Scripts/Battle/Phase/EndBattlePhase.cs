using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class EndBattlePhase : BattlePhase
{
    bool m_is_clear = false;

    public EndBattlePhase(BattlePhaseEnum phase_enum) : base(phase_enum) { }

    public override void OnStateEnter()
    {
        m_duration = 0;
        bool has_dead = false;
        bool has_player = false;
        bool has_hero = false;
        var bm = BattleManager.Instance;
        var ui = bm.BattleUI;

        ui.SkillTargetingOff();
        bm.UnitWaitingDecrease();

        

        for (int i = bm.WaitingUnitsList.Count - 1; i >= 0; i--)
        {
            var unit = bm.WaitingUnitsList[i];
            if (unit.IsDead)
            {
                has_dead = true;
                has_player = unit.IsPlayerUnit || has_player;
                has_hero = (!unit.IsPlayerUnit) || has_hero;
                bm.UnitDestroy(unit);
                ui.OnUnitDead(unit);
            }
        }

        if (has_dead)
        {
            if (has_player)
                bm.ReorderUnit(true);
            if (has_hero)
                bm.ReorderUnit(false);
        }
        if (bm.PlayerTeam.Count == 0)
        {
            bm.StageFail();
        }
        if (bm.HeroTeam.Count == 0)
        {
            m_is_clear = true;
        }

        m_duration = Duration;
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
        if (m_is_clear == false)
            return;
        m_is_clear = false;
        var bm = BattleManager.Instance;
        if (bm.MoveNextStage() == null)
            bm.StageClear();
    }
    public override void OnStateInit()
    {
        AddTransition((int)BattlePhaseEnum.ReadyBattlePhase, IsDurationExpired);
    }
}