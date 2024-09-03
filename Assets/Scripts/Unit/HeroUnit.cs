using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroUnit : UnitBase
{
    public HeroUnit() : base()
    {
        IsPlayerUnit = false;
    }
    public HeroUnit(UnitBase unitBase) : base(unitBase)
    {
        IsPlayerUnit = false;
    }
    public void Behave()
    {
        SkillList[0].Invoke(this, BattleManager.Instance.PlayerTeam[0]);
    }
}
