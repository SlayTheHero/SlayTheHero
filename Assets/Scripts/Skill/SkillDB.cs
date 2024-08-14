using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Skill
{
    /// <summary>
    /// 스킬의 작용에 따른 타입
    /// </summary>
    public enum SK_BehaviorType
    {
        Melee,           // ex. 이동
        Projectile,      // ex. 투사체
        Buff,             // ex. 투사체 없이 타겟에게 특정 애니메이션
        Special           // 특수
    }

    /// <summary>
    /// 스킬의 지속시간에 따른 타입
    /// </summary>
    public enum SK_DurationType
    {
        Passive, Active, Buff, Special
    }
    /// <summary>
    /// 변화시킬 캐릭터의 속성
    /// </summary>
    public enum SK_Attribute
    {
        Speed, MaxHealth ,Health, Attack, Defence, Resist,  // 속도, MaxHP ,HP, 공격력, 방어력, 저항
        CritChance, CritDamage, Penetration, StunChance, ConfusionChance, Dodge, // 크리확률, 크리뎀, 방어관통, 스턴확률, 혼란확률, 회피확률
        BaseAttack, Skill_1, Skill_2, Skill_3, // 행동
        Stun, Confusion
    }
    /// <summary>
    /// 변화가 증감인지 배수인지
    /// </summary>
    public enum SK_ChangeType
    {
        Modification, Multiplication
    }

    // 기본정보
    public int id;
    public string name;
    public string description;

    // 타입정보
    public SK_BehaviorType sK_BehaviorType;
    public SK_DurationType sk_DurationType;
    public SK_Attribute sK_Attribute;
    public SK_ChangeType sK_ChangeType;

    // 애니메이션?

    // 세부정보
    public int range; // 캐릭터부터 사용할 수 있는 최대 거리
    public int impact; // 스킬의 강도.변화타입이 Multiplier 일땐 100을 나누어 변환 ex. 120이면 1.2 배
    public int duration; //  스킬 지속시간 
    public int coolTime; //  스킬 쿨타임
    public int nowCoolDown = 0; // 현재 쿨타임
    public int nowDuration = 0; // 현재 지속시간




    /// <summary>
    /// 스킬 생성자
    /// </summary>
    /// <param name="id">스킬 코드</param>
    /// <param name="name">스킬 이름</param>
    /// <param name="description">스킬 설명</param>
    /// <param name="sK_BehaviorType">스킬 행동 타입</param>
    /// <param name="sk_DurationType">스킬 지속시간 타입</param>
    /// <param name="sK_Attribute">변화시킬 캐릭터의 속성</param>
    /// <param name="sK_ChangeType">변화 타입</param>
    /// <param name="range">스킬 범위</param>
    /// <param name="impact">스킬 강도</param>
    /// <param name="duration">스킬 지속시간</param>
    /// <param name="coolTime">스킬 쿨타임</param>
    public Skill(
        int id,
        string name,
        string description,
        SK_BehaviorType sK_BehaviorType,
        SK_DurationType sk_DurationType,
        SK_Attribute sK_Attribute,
        SK_ChangeType sK_ChangeType,
        int range,
        int impact,
        int duration,
        int coolTime
    )
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.sK_BehaviorType = sK_BehaviorType;
        this.sk_DurationType = sk_DurationType;
        this.sK_Attribute = sK_Attribute;
        this.sK_ChangeType = sK_ChangeType;
        this.range = range;
        this.impact = impact;
        this.duration = duration;
        this.coolTime = coolTime;
    }
    // 복사 생성자
    public Skill(Skill other)
    {
        this.id = other.id;
        this.name = other.name;
        this.description = other.description;
        this.sK_BehaviorType = other.sK_BehaviorType;
        this.sk_DurationType = other.sk_DurationType;
        this.sK_Attribute = other.sK_Attribute;
        this.sK_ChangeType = other.sK_ChangeType;
        this.range = other.range;
        this.impact = other.impact;
        this.duration = other.duration;
        this.coolTime = other.coolTime;
    }
    public void Invoke(UnitBase Attacker, UnitBase Target)
    {
        SkillExecuter.Execute(Attacker,Target,this);
    }
    UnitBase blankUnit = new UnitBase();
    public void Invoke(UnitBase Target)
    {
        SkillExecuter.Execute(blankUnit, Target, this);
    }
}
public static class SkillDB
{
    /// <summary>
    /// 유닛의 타입별로 스킬 정보가 할당
    /// </summary>
    public static Dictionary<string, List<int>> SkillTypeData = new Dictionary<string, List<int>>();
    private static List<Skill> SkillList = new List<Skill>();
    /// <summary>
    /// 스킬 ID를 통해 스킬 정보를 가져오는 함수
    /// </summary>
    /// <param name="id">스킬 ID</param>
    /// <returns></returns>
    public static Skill GetSkill(int id)
    {
        if (SkillList.Count == 0)
        {
            initializeSkillList();
        }

        if (id >= SkillList.Count)
        {
            Debug.Log($"{id} is not Valid Skill ID");
            return null;
        }

        return new Skill(SkillList[id]);
    }
    public static void initializeSkillList()
    {
        if (SkillList.Count != 0) return;

        List<Dictionary<string, object>> dict = CSVReader.Read("Csvs/SkillInfo");
        foreach (Dictionary<string, object> item in dict)
        {
            string type = (string)item["Type"];
            int id = (int)item["Skill_ID"];
            if(SkillTypeData.ContainsKey(type))
            {
                SkillTypeData[type].Add(id);
            }
            else
            {
                SkillTypeData.Add(type,new List<int>() { id });
            }

            string name = (string)item["Name"];
            string description = (string)item["Description"];
            Skill.SK_DurationType durType = Utility.StringToEnum<Skill.SK_DurationType>((string)item["DurationType"]);
            Skill.SK_BehaviorType Behavtype = Utility.StringToEnum<Skill.SK_BehaviorType>((string)item["BehaviorType"]);
            Skill.SK_Attribute attr = Utility.StringToEnum<Skill.SK_Attribute>((string)item["Attribute"]);
            Skill.SK_ChangeType cType = Utility.StringToEnum<Skill.SK_ChangeType>((string)item["ChangeType"]);
            int range = (int)item["Range"];
            string tempImpact = item["Impact"].ToString();
            int impact = 0;
            if (tempImpact.Contains("{Attack}"))
            {
                impact = 0;
            }
            else
            {
                impact = (int)item["Impact"];
            }
            int duration = (int)item["Duration"];
            int coolDown = (int)item["CoolTime"];

            Skill tempSkill = new Skill(id, name, description, Behavtype, durType, attr, cType, range, impact,duration,coolDown);
            SkillList.Add(tempSkill);
        }
    }
}

