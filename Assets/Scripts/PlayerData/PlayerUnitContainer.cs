using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CharacterSelect Scene에서 BattleScene 으로 넘어갈 때 사용됩니다.,
/// </summary>
public static class PlayerUnitContainer
{
    private static List<UnitBase> selectUnitList = new List<UnitBase>();

    public static void AddUnitList(UnitBase unit)
    {
        selectUnitList.Add(new UnitBase(unit));
    }
    public static List<UnitBase> GetUnitList()
    {
        if(selectUnitList.Count == 0)
        {
            selectUnitList.Add(UnitDB.GetUnit(0));
            selectUnitList.Add(UnitDB.GetUnit(1));
            selectUnitList.Add(UnitDB.GetUnit(2));
        }
        return selectUnitList;
    }
    public static void ClearUnitList()
    {
        selectUnitList.Clear();
    }
}
