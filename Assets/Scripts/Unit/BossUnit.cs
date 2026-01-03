using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossUnit : HeroUnit
{
    public Action Hit;
    public int rage_count;
    int skill1_delay;
    bool is_fury;
    enum State
    {
        Default, Rage, Fury
    }
    State m_CurState;

    public BossUnit(HeroUnit other)
    {
        Position = other.Position;
        ID = other.ID;
        Name = other.Name;
        Status = new Status(other.Status);
        Job = other.Job;
        Feature = other.Feature;
        Race = other.Race;
        SkillList = new List<Skill>();
        for (int i = 0; i < other.SkillList.Count; i++)
        {
            SkillList.Add(other.SkillList[i]);
        }
        BuffController = new BuffController(other.BuffController);

        m_CurState = State.Default;

    }
    public void OnHit()
    {
        if (BattleManager.Instance.CurUnit.Position == 1)
        {
            rage_count--;
        }
        else
        {
            rage_count = 3;
            m_CurState = State.Rage;
        }
        if (rage_count == 0)
        {
            m_CurState = State.Default;
        }
        if (Status.HP / Status.MaxHP < 50f)
        {
            m_CurState = State.Fury;
        }
    }
    public override void Behave()
    {
            var bm = BattleManager.Instance;
        switch (m_CurState)
        {
            case State.Default:
                bm.SelectedSkillNum = 2;
                bm.SkillUsed.Invoke(bm.PlayerTeam.First());
                return;
            case State.Rage:
                skill1_delay--;
                if (skill1_delay == 0)
                {
                    bm.SelectedSkillNum = 1;
                    bm.SkillUsed.Invoke(bm.PlayerTeam.First());
                    skill1_delay = 2;
                }
                else
                {
                    bm.SelectedSkillNum = 0;
                    bm.SkillUsed.Invoke(bm.PlayerTeam.First());
                }
                return;
            case State.Fury:
                if (is_fury)
                {
                    bm.SelectedSkillNum = 0;
                    bm.SkillUsed.Invoke(bm.PlayerTeam.First());
                }
                else
                {
                    is_fury = true;
                    bm.SelectedSkillNum = 2;
                    bm.SkillUsed.Invoke(this);
                }
                return;
        }
        //보유 스킬중 랜덤으로 사용
        //사용 범위 고려
        BattleManager.Instance.TurnSkip.Invoke();
    }
}
