using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public static class UnitDB 
{

    private static List<UnitBase> UnitList = new List<UnitBase>();
    private static List<Dictionary<string, object>> UnitDict;

    // '-' 가 입력 되었을때
    private const int RANDOM_SKILL_VALUE = int.MaxValue;    
    // 타입 이름이 입력되었을때
    private const int TYPE_SKILL_VALUE = int.MinValue;

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
        // 종족 랜덤 부여
        if (outunit.Job == Job.Random)
        {
            outunit.Job = GetRandomEnumValue<Job>(Job.Magician);
            outunit.SkillList[1] = GetRandomSkill<Job>(outunit.Job);
        }
        //특성 랜덤 부여
        if (outunit.Feature == Feature.Random)
        {
            outunit.Feature = GetRandomEnumValue<Feature>(Feature.Envy);
            outunit.SkillList[3] = GetRandomSkill<Feature>(outunit.Feature);
        }
        //스킬 랜덤 부여
        for (int i = 0; i < outunit.SkillList.Count; i++)
        {
            Skill now = outunit.SkillList[i];
            if(now.id == TYPE_SKILL_VALUE)
            {
                string typeName = now.name;
                int random = UnityEngine.Random.Range(0, SkillDB.SkillTypeData[typeName].Count);
                outunit.SkillList[i] = SkillDB.GetSkill(SkillDB.SkillTypeData[typeName][random]);
            }
            else if (now.id == RANDOM_SKILL_VALUE)
            {
                switch(i)
                {
                    case 0:
                        break;
                    case 1:
                        outunit.SkillList[i] = GetRandomSkill<Job>(outunit.Job);
                        break;
                    case 2:
                        outunit.SkillList[i] = GetRandomSkill<Race>(outunit.Race);
                        break;
                    case 3:
                        outunit.SkillList[i] = GetRandomSkill<Feature>(outunit.Feature);
                        break;
                }

            }
        }
        //외형 부여
        outunit.outFit = OutfitChooser.GetOutFitFromUnit(outunit);

        return outunit;
    }
    
    public static UnitBase GetUnitForSynergy(Synergy synergy)
    {
        return new UnitBase();
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
                if (tempSkill == "-")
                {
                    skills.Add(new Skill(RANDOM_SKILL_VALUE));
                    continue;
                }
                if(SkillDB.SkillTypeData.ContainsKey(tempSkill))
                {
                    Skill temp = new Skill(tempSkill);
                    temp.id = TYPE_SKILL_VALUE;
                    skills.Add(temp);
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
    private static T GetRandomEnumValue<T>(T targetMax) where T : Enum
    {
        int maxValue = Convert.ToInt32(targetMax);  
        int rand = UnityEngine.Random.Range(0, maxValue + 1);  
        return (T)Enum.ToObject(typeof(T), rand);  
    }

    private static Skill GetRandomSkill<T>(T enumValue) where T : Enum
    {
        if (!SkillDB.SkillTypeDataForenum.ContainsKey(enumValue))
        {
            return SkillDB.GetSkill(0);
        }

        int rand = UnityEngine.Random.Range(
            SkillDB.SkillTypeDataForenum[enumValue].Min(),
            SkillDB.SkillTypeDataForenum[enumValue].Max() + 1
        ); 
        return SkillDB.GetSkill(rand);
    }
       
}
