using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager : MonoBehaviour
{
    
    
    
    void SetPlayerTeamForTest()
    {
        PlayerTeam = new();
        UnitDB.initializeUnitList();
        for (int i = 0; i < 3; i++)
        {
            PlayerTeam.Add(new PlayerUnit(UnitDB.GetUnit(0)));
            TurnList.Add(PlayerTeam[i]);
        }
    }
}
