using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public PlayerUnit() : base()
    {
        IsPlayerUnit = true;
    }
    public PlayerUnit(UnitBase unitBase) : base(unitBase)
    {
        IsPlayerUnit = true;
    }
}
