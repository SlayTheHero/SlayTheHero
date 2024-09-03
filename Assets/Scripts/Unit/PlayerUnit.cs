using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerUnit : UnitBase
{
    public PlayerUnit(UnitBase unitBase) : base(unitBase)
    {
        IsPlayerUnit = true;
    }
}
