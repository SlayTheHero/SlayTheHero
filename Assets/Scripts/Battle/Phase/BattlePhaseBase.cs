using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum BattlePhaseEnum
{
    ReadyBattlePhase, BattlePhase, EndBattlePhase
}
[Serializable]
public abstract class BattlePhaseBase : fsm.State
{
    protected float m_duration;
    public static Dictionary<BattlePhaseEnum, BattlePhaseBase> BattlePhaseTable = new();
    public float Duration;
    protected bool IsDurationExpired()
    {
        return m_duration < 0;
    }
    public BattlePhaseBase(BattlePhaseEnum phase_enum) : base((int)phase_enum)
    {
        BattlePhaseTable.Add(phase_enum, this);
    }
    public BattlePhaseBase SetDuration(float duration)
    {
        this.Duration = duration;
        return this;
    }
}