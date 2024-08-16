using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyController
{
    /// <summary>
    /// �ó����� Ȱ��ȭ�մϴ�.
    /// </summary>
    /// <param name="synergy">�ó��� ����</param>
    /// <param name="isThreeMember">true�̸� 3�� flase�̸� 2�� �Դϴ�.</param>
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
