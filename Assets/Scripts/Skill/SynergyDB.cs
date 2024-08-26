using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Synergy : Skill
{
    public enum SynergyType
    {
        Vampire,DemonBeast,NightMare,Ghost, 
        SwordMan,Archer,Magician,
        Swiftness,SuspiciousGhost,Sloth,Envy,
    }
    public int twoImpact;
    public int threeImpact;

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
            return null;
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
}

