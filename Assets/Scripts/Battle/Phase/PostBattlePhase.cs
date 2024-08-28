using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PostBattlePhase : Phase
{
    public PostBattlePhase(Phases _step) : base(_step)
    {
    }

    public override void OnEnterPhase()
    {

        Debug.Log("Enter PostBattlePhase");
        var bm = BattleManager.Instance;
        if(bm.HeroTeam.Count == 0 || bm.PlayerTeam.Count == 0)
        {
            //스테이지 클리어 or 실패
        }
        else
        {
            for(int i = 1; i < bm.TurnList.Count;i++)
            {
                var item = bm.TurnList[i];
                item.Status.Waiting -= bm.StagedUnit.Status.Waiting > 0 ? bm.StagedUnit.Status.Waiting : 0;
            }
            
            bm.StagedUnit.Status.Waiting = (int)((1f / bm.StagedUnit.Status.Speed)*10000);
            bm.StagedUnit.Turn++;
            bm.ChangePhase(Phases.PreBattlePhase);
        }
    }

    public override void OnExitPhase()
    {
        
    }
}
