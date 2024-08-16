using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class StageDB
{
    public const string StageDBPath = "CSVs/BattleSceneDB";
    public static Dictionary<int,StageData> AllStages;
    public static StageData GetStageData(int stage)
    {
        if (AllStages == null)
        {
            AllStages = CSVReader.ReadStageData(StageDBPath);
        }
        if (AllStages.ContainsKey(stage))
        {
            StageData stageData = AllStages[stage];
            return stageData;
        }
        return null;
    }
}
public class StageData
{
    public int ID;
    public int SubStageID;
    public List<HeroUnit> HeroUnits;
    public StageData(int stageID , int subID, List<HeroUnit> h)
    {
        ID = stageID;
        SubStageID = subID;
        HeroUnits = h;
    }
}