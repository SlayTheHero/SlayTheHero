using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager : MonoBehaviour
{
    void UnitGenerateForSpeedTest()
    {
        GameObject pu = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        GameObject hu = Resources.Load<GameObject>("Prefabs/HeroUnit");
        PlayerTeam = new List<PlayerUnit>();
        HeroTeam = new List<HeroUnit>();
        TurnList = new List<UnitBase>();
        for (int i = 0; i < 3; i++)
        {
            var p = Instantiate(pu).GetComponent<PlayerUnit>();
            p.Status.Speed = i;
            p.Name = "player_unit_" + i;
            PlayerTeam.Add(p);
            TurnList.Add(p);
            var h = Instantiate(hu).GetComponent<HeroUnit>();
            h.Status.Speed = i;
            h.Name = "hero_unit_" + i;
            HeroTeam.Add(h);
            TurnList.Add(h);
        }
    }
    void SpeedSortTest()
    {
        UnitGenerateForSpeedTest();
        SortUnitOrder();
    }
    void SkillGenerate()
    {

    }
    void SetHeroTeamForTest()
    {
        UnitDB.initializeUnitList();
        for (int i = 0; i < 3; i++)
        {
            HeroTeam.Add(UnitDB.GetUnit(0) as HeroUnit);
        }
    }
}
