using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public static class UnitDB 
{

    private static List<UnitBase> UnitList = new List<UnitBase>();
    private static List<Dictionary<string, object>> UnitDict;
    /// <summary>
    /// 유닛 ID를 통해 유닛 정보를 가져오는 함수
    /// </summary>
    /// <param name="id">스킬 ID</param>
    /// <returns></returns>
    public static UnitBase GetUnit(int id)
    {
        if (UnitList.Count == 0)
        {
            initializeUnitList();
        }

        if (id >= UnitList.Count)
        {
            Debug.Log($"{id} is not Valid Unit ID");
            return null;
        }
        UnitBase unit = UnitList[id];
        // 값 랜덤설정 후 유닛 반환 
        ((RandomStatus)unit.Status).setRandomValue();
        UnitBase outunit = new UnitBase(unit);
        if (outunit.Feature == Feature.Random)
        {
            outunit.Feature = getRandomFeature();
        }
        return outunit;
    }
    public static void initializeUnitList()
    {
        if (UnitList.Count != 0) return;

        UnitDict = CSVReader.Read("Csvs/UnitInfo"); 
        foreach (Dictionary<string, object> item in UnitDict)
        {
            int id = (int)item["Unit_ID"];
            string name = item["Name"].ToString();
            Race race = Utility.StringToEnum<Race>(item["Race"].ToString());
            Job job = Utility.StringToEnum<Job>(item["Job"].ToString());
            Feature feat = Utility.StringToEnum<Feature>(item["Feature"].ToString());

            (int,int) speed = getintTupleValue("Speed");
            (int,int) HP = getintTupleValue("HP");
            (int,int) atk = getintTupleValue("Attack");
            (int,int) def = getintTupleValue("Defence");
            (int,int) Res = getintTupleValue("Resistance");
            (int,int) CritChance = getintTupleValue("CritChance");
            (int,int) CritDamage = getintTupleValue("CritDamage");
            (int,int) pene = getintTupleValue("Penetration");
            (int,int) stun = getintTupleValue("StunChance");
            (int,int) confu = getintTupleValue("ConfusionChance");
            (int,int) dodge = getintTupleValue("DodgeChance");

            Status status = new RandomStatus(HP, HP, atk, def, pene, Res, CritChance, CritDamage, stun, confu, dodge, speed);
            List<Skill> skills = new List<Skill>();
            for (int i = 0; i < 4; i++)
            {
                string tempSkill = item[$"Skill_Id_{i}"].ToString();
                if (tempSkill == "-") continue;
                if(SkillDB.SkillTypeData.ContainsKey(tempSkill))
                {
                    int random = UnityEngine.Random.Range(0, SkillDB.SkillTypeData[tempSkill].Count);
                    skills.Add(SkillDB.GetSkill(SkillDB.SkillTypeData[tempSkill][random]));
                }
                else
                {
                    int temp = -1;
                    if(!Int32.TryParse(tempSkill,out temp))
                    {
                        Debug.Log($"{tempSkill} is not valid Skill Code");
                        continue;
                    }
                    skills.Add(SkillDB.GetSkill(temp));
                }
            }

            UnitBase tempUnit = new UnitBase(id, name, status, job, feat, race, skills);
            UnitList.Add(tempUnit);

            (int,int) getintTupleValue(string type)
            {
                string temp = item[type].ToString();
                if (temp.Contains('~'))
                { 
                    return getRangeInfo(temp);
                    
                }
                else
                {
                    int value = (int)item[type];
                    return (value,value);
                }
            }

        }

    }
     
    private static (int,int) getRangeInfo(string input)
    {
        int[] minMax = Array.ConvertAll(input.Split('~'), int.Parse);
        return (minMax[0], minMax[1]);
    }
    private static Feature getRandomFeature()
    {
        int rand = UnityEngine.Random.Range(0, (int)Feature.Envy + 1);
        return (Feature)rand;
    }
}
