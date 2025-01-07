using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public int Behave()
    {
        SkillList[0].Invoke(this,BattleManager.Instance.HeroTeam.Last());
        return 5;
    }
}
