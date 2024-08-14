using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyController
{
    /// <summary>
    /// 시너지를 활성화합니다.
    /// </summary>
    /// <param name="synergy">시너지 종류</param>
    /// <param name="isThreeMember">true이면 3명 flase이면 2명 입니다.</param>
    public void setSynergy(UnitBase unit,Synergy.SynergyType synergy,bool isThreeMember)
    {
        Synergy sy = SynergyDB.GetSynergy((int)synergy);
        int impact;
        if(isThreeMember)
        {
            impact = sy.threeImpact;
        }
        else
        {
            impact = sy.twoImpact;
        }
        sy.impact = impact;

        unit.BuffController.AddBuff(sy);

    }

}
