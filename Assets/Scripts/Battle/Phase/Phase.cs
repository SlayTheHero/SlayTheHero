using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phases
{
    PreBattlePhase,BattlePhase,PostBattlePhase
}

public abstract class Phase
{
    public static Dictionary<Phases, Phase> PhaseTable = new();
    Phases step;
    public Phases GetStep { get => step; }
    public Phase(Phases _step)
    {
        PhaseTable[_step] = this;
        step = _step;
    }

    public abstract void OnEnterPhase();
    public abstract void OnExitPhase();
    
}
