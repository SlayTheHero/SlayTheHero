using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Synergy : Skill
{
    public int twoImpact;
    public int threeImpact;

    public enum SynergyType
    {
        Vampire, DemonBeast, NightMare, Ghost, Human, // 종족
        SwordMan, Archer, Magician, // 직업
        Farmer, Mercenary, Knight, Paladin, Priest, // 적 직업
        Swiftness, SuspiciousGhost, Sloth, Envy, // 아군 특성
        Justice, Hunter, Dreamless, Fortify, Noble, Faithful, // 적군 특성
    }
    public Synergy()
    {

    }
    // 파라미터 생성자
    public Synergy(
        int id,
        string name,
        string description,
        SK_DurationType sk_DurationType,
        SK_Attribute sk_Attribute,
        SK_ChangeType sk_ChangeType,
        int twoImpact,
        int threeImpact
    ) : base(
         id, name,  description,
         SK_BehaviorType.Special,
         sk_DurationType,
         sk_Attribute,
         sk_ChangeType,
         0, 0, 0, 0)
    {
        this.id = id;
        this.name = name;
        this.description = description; 
        this.sk_DurationType = sk_DurationType;
        this.sK_Attribute = sk_Attribute;  
        this.twoImpact = twoImpact;
        this.threeImpact = threeImpact;

    }
    // 복사 생성자
    public Synergy(Synergy other) : base(other)
    {
        this.twoImpact = other.twoImpact;
        this.threeImpact = other.threeImpact;
    }

    public static Enum FromSynergyToUnitEnum(SynergyType synergy)
    {
        if (Enum.IsDefined(typeof(Job), synergy.ToString()))
        {
            return (Job)Enum.Parse(typeof(Job), synergy.ToString());
        }
        if (Enum.IsDefined(typeof(Feature), synergy.ToString()))
        {
            return (Feature)Enum.Parse(typeof(Feature), synergy.ToString());
        }
        if (Enum.IsDefined(typeof(Race), synergy.ToString()))
        {
            return (Race)Enum.Parse(typeof(Race), synergy.ToString());
        }
        throw new ArgumentException($"SynergyType {synergy} does not map to a known enum value.");
    }

    public static SynergyType FromUnitEnumToSynergy(Enum enumValue)
    { 
        if (enumValue is Enum)
        {
            string enumName = enumValue.ToString();

            // enumName이 Job, Feature, Race 중 하나에 속하는지 확인
            if (Enum.IsDefined(typeof(Job), enumName) ||
                Enum.IsDefined(typeof(Feature), enumName) ||
                Enum.IsDefined(typeof(Race), enumName))
            {
                // enumName이 SynergyType에도 존재하면 변환
                if (Enum.IsDefined(typeof(SynergyType), enumName))
                {
                    return (SynergyType)Enum.Parse(typeof(SynergyType), enumName);
                }
            }
        }
        throw new ArgumentException($"The enum value {enumValue} does not map to SynergyType.");
    }


}
public static class SynergyDB
{ 
    private static List<Synergy> SynergyList = new List<Synergy>();
    /// <summary>
    /// 스킬 ID를 통해 스킬 정보를 가져오는 함수
    /// </summary>
    /// <param name="id">스킬 ID</param>
    /// <returns></returns>
    public static Synergy GetSynergy(int id)
    {
        if (SynergyList.Count == 0)
        {
            initializeSynergyList();
        }

        if (id >= SynergyList.Count)
        {
            Debug.Log($"{id} is not Valid Synergy ID");
            return new Synergy();
        }

        return new Synergy(SynergyList[id]);
    }
    public static void initializeSynergyList()
    {
        if (SynergyList.Count != 0) return;

        List<Dictionary<string, object>> dict = CSVReader.Read("Csvs/SynergyInfo");
        foreach (Dictionary<string, object> item in dict)
        {
            string type = (string)item["Type"];
            int id = (int)item["Synergy_ID"];
            string name = (string)item["Name"];
            string description = (string)item["Description"];
            Skill.SK_DurationType durType = Utility.StringToEnum<Skill.SK_DurationType>((string)item["DurationType"]);
            Skill.SK_Attribute attr = Utility.StringToEnum<Skill.SK_Attribute>((string)item["Attribute"]);
            Skill.SK_ChangeType cType = Utility.StringToEnum<Skill.SK_ChangeType>((string)item["ChangeType"]);
            int twoImpact = getIntValueOrZero("TwoImpact");
            int threeImpact = getIntValueOrZero("ThreeImpact");
            Synergy tempSkill = new Synergy(id, name, description, durType, attr, cType, twoImpact,threeImpact);
            SynergyList.Add(tempSkill);
            
            
            int getIntValueOrZero(string type)
            {
                if (item[type] == "")
                {
                    return 0;
                }
                else
                {
                    return (int)item[type];
                }
            }
        }
    }

    /// <summary>
    /// getSynergyList from unitList. int is SynergyIndex, bool is 2nd or 3rd Synergy. if true. it is 3rd synergy.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static List<(int,bool)> getSynergyFromUnitList(List<UnitBase> unit)
    {
        if (unit.Count == 0)
        {
            Debug.Log("Error : UnitList is Empty");
            return null; 
        }

        List<(int,bool)> synergyList = new List<(int, bool)>();

        if (unit.Count > 1)
        {
            short[] raceCount = new short[Enum.GetValues(typeof(Race)).Length];
            short[] featCount = new short[Enum.GetValues(typeof(Feature)).Length];
            short[] jobCount = new short[Enum.GetValues(typeof(Job)).Length];

            // 유닛 정보에서 카운트를 셈
            for (int i = 0; i < unit.Count; i++)
            {
                UnitBase nowUnit = unit[i];
                raceCount[(int)nowUnit.Race]++;
                featCount[(int)nowUnit.Feature]++;
                jobCount[(int)nowUnit.Job]++;
            }

            // 카운트를 기준으로 시너지 계산
            (int, bool) raceSynergy = ExtractMax<Race>(raceCount);
            (int, bool) jobSynergy = ExtractMax<Job>(jobCount);
            (int, bool) featSynergy = ExtractMax<Feature>(featCount);

            if(raceSynergy.Item1 != -1)
            {
                synergyList.Add(raceSynergy);
            }
            if (jobSynergy.Item1 != -1)
            {
                synergyList.Add(jobSynergy);
            }
            if (featSynergy.Item1 != -1)
            {
                synergyList.Add(featSynergy);
            }
        }

        return synergyList;
    }

    // 공통 로직을 처리하는 함수
    private static (int, bool) ExtractMax<T>(short[] countArray) where T : Enum
    { 
        int max = 0;
        int maxIndex = 0;

        // 최대값 찾기
        for (int i = 0; i < countArray.Length; i++)
        {
            if (countArray[i] > max)
            {
                max = countArray[i];
                maxIndex = i;
            }
        }

        T enumValue = (T)(object)maxIndex; // maxIndex를 T로 캐스팅
        int synergy = (int)Synergy.FromUnitEnumToSynergy(enumValue);
        switch (max)
        {
            case 1:
                return (-1, false);
            case 2:
                return (synergy, false);
            case 3:
                return (synergy, true);
            default:
                return (-1, true);
        }

    }

}

