using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public static class UnitDB 
{

    private static List<UnitBase> UnitList = new List<UnitBase>();
    private static List<Dictionary<string, object>> UnitDict;
    private static readonly Dictionary<Job,List<int>> JOB_STAT_DATA = new Dictionary<Job,List<int>>
    {    
        // speed, health, attack, defence
        { Job.Archer, new List<int> { 150, 80, 120, 100 } },
        { Job.SwordMan, new List<int> { 80, 150, 100, 150 } },
        { Job.Magician, new List<int> { 100, 80, 180, 80 } }
    };
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
    
    public static UnitBase GetUnitForSynergy(Synergy.SynergyType synergy)
    { 
        UnitBase unit = new UnitBase();
        bool isEnemy = true;
        Type type = typeof(int);
        // 적군 아군 판별 
        if (Enum.IsDefined(typeof(Job), synergy.ToString()))
        {
            Job now = (Job)Enum.Parse(typeof(Job), synergy.ToString());
            type = typeof(Job);
            if(now < Job.Farmer)
            {
                isEnemy = false;
            } 
        }
        if (Enum.IsDefined(typeof(Feature), synergy.ToString()))
        {
            Feature now = (Feature)Enum.Parse(typeof(Feature), synergy.ToString());
            type = typeof(Feature);
            if (now < Feature.Justice)
            {
                isEnemy = false;
            }
        }
        if (Enum.IsDefined(typeof(Race), synergy.ToString()))
        {
            Race now = (Race)Enum.Parse(typeof(Race), synergy.ToString());
            type = typeof(Race);
            if (now < Race.Human)
            {
                isEnemy = false;
            }
        } 

        //랜덤 삽입
        if(isEnemy)
        {
            unit.Job = GetRandomEnumValue<Job>(Job.Priest, Job.Farmer);
            unit.Feature = GetRandomEnumValue<Feature>(Feature.Faithful, Feature.Justice); ;
            unit.Race = Race.Human;
        }
        else
        {
            unit.Job = GetRandomEnumValue<Job>(Job.SwordMan, Job.Magician);
            unit.Feature = GetRandomEnumValue<Feature>(Feature.Swiftness, Feature.Envy);
            unit.Race = GetRandomEnumValue<Race>(Race.Ghost);
        }

        switch (type)
        {
            case Type t when t == typeof(Job):
                unit.Job = (Job)Enum.Parse(typeof(Job), synergy.ToString());
                break;
            case Type t when t == typeof(Feature):
                unit.Feature = (Feature)Enum.Parse(typeof(Feature), synergy.ToString());
                break;
            case Type t when t == typeof(Race):
                unit.Race = (Race)Enum.Parse(typeof(Race), synergy.ToString());
                break;
            default:
                Debug.LogError("Unexpected type");
                break;
        } 
        //직업군 분류
        Job nowStatType = unit.Job;

        //직업 특성 분류
        if (isEnemy)
        {
            switch(unit.Job)
            {
                case Job.Farmer:
                    nowStatType = Job.SwordMan;
                    break;
                case Job.Mercenary:
                    nowStatType = Job.SwordMan;
                    break;
                case Job.Knight:
                    nowStatType = Job.Archer;
                    break;
                case Job.Paladin:
                    nowStatType = Job.Archer;
                    break;
                case Job.Priest:
                    nowStatType = Job.Priest;
                    break;
                case Job.Magician:
                    nowStatType = Job.Magician;
                    break;
                default:
                    break;
            }
        }

        //스탯부여
        List<int> stat = JOB_STAT_DATA[nowStatType];
        unit.Status.Speed = stat[0];
        unit.Status.MaxHP = stat[1];
        unit.Status.HP = stat[1];
        unit.Status.ATK = stat[2];
        unit.Status.DEF = stat[3];
         
        //스킬 랜덤 부여
        for (int i = 0; i < 4; i++)
        {
            Skill now = new Skill();
            switch (i)
            {
                case 0:
                    now = GetPassiveSkill(unit.Job);
                    break;
                case 1:
                    now = GetRandomSkill<Job>(unit.Job);
                    break;
                case 2:
                    now = GetRandomSkill<Race>(unit.Race);
                    break;
                case 3:
                    now = GetRandomSkill<Feature>(unit.Feature);
                    break;
            }
            unit.SkillList.Add(now);
        }
        //외형 부여
        unit.outFit = OutfitChooser.GetOutFitFromUnit(unit);

        return unit;
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
    private static T GetRandomEnumValue<T>(T targetMax, T targetMin = default(T)) where T : Enum
    {
        int minValue = Convert.ToInt32(targetMin);
        int maxValue = Convert.ToInt32(targetMax);
        int rand = UnityEngine.Random.Range(minValue, maxValue + 1);
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
       

    private static Skill GetPassiveSkill(Job enumValue)
    {
        Job nowStatType = enumValue;

        //직업 특성 분류
        if (nowStatType > Job.Magician)
        {
            switch (enumValue)
            {
                case Job.Farmer:
                    nowStatType = Job.SwordMan;
                    break;
                case Job.Mercenary:
                    nowStatType = Job.SwordMan;
                    break;
                case Job.Knight:
                    nowStatType = Job.Archer;
                    break;
                case Job.Paladin:
                    nowStatType = Job.Archer;
                    break;
                case Job.Priest:
                    nowStatType = Job.Priest;
                    break;
                case Job.Magician:
                    nowStatType = Job.Magician;
                    break;
                default:
                    break;
            }
        }

        switch(nowStatType)
        {
            case Job.SwordMan:
                return SkillDB.GetSkill(0);
            break;
            case Job.Archer:
                return SkillDB.GetSkill(1);
                break;
            case Job.Magician:
                return SkillDB.GetSkill(2);
                break;
            default:
                return new Skill();
                break;
        }
    }
}
