using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Synergy;

public static class OutfitChooser 
{ 
    public static OutFit GetOutFitFromUnit(UnitBase unit)
    {
        if(unit.Race == Race.Human) 
        {
            return (OutFit)Enum.Parse(typeof(OutFit), unit.Job.ToString());
        }
        else
        {
            return (OutFit)Enum.Parse(typeof(OutFit), unit.Race.ToString());
        }
    }
}
