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
        if (selectUnitList.Count == 0)
        {
            selectUnitList.Add(UnitDB.GetUnit(0));
            selectUnitList.Add(UnitDB.GetUnit(0));
            selectUnitList.Add(UnitDB.GetUnit(0));
        }
        else
        {

        }
        
        SetBuffController();

        return selectUnitList;
    }

    private static void SetBuffController()
    {
        List<(int,bool)> synergyData = SynergyDB.getSynergyFromUnitList(selectUnitList);

        for (int i = 0; i < 3; i++)
        {
            selectUnitList[i].BuffController.Init(selectUnitList[i]);
            for (int j = 0; j < synergyData.Count; j++)
            {
                SynergyController.setSynergy(selectUnitList[i], (Synergy.SynergyType)synergyData[j].Item1, synergyData[j].Item2);
            }
        }
    }

    public static void ClearUnitList()
    {
        selectUnitList.Clear();
    }
}
