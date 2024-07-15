using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public List<PlayerUnit> PlayerTeam;
    public List<HeroUnit> HeroTeam;
    public Queue<UnitBase> TurnQueue;
    public UnitBase StagedUnit;
    public BattleUI BattleUI;
    public UnityEvent OnTurnEnd;
    // Start is called before the first frame update
    void Start()
    {

    }

    void LoadHeroTeam()
    {

    }

    void SetUnitOrder()
    {

    }
    void StagingUnitChange()
    {

    }
    void BattleEnd()
    {

    }
}
