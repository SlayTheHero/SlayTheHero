using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct Skill
{
    /// <summary>
    /// 스킬의 타입
    /// </summary>
    public enum SK_Type
    {
        Passive, Active, Buff, Special
    }
    /// <summary>
    /// 변화시킬 캐릭터의 속성
    /// </summary>
    public enum SK_Attribute
    {
        Speed, Health, Attack, Defence, Resist,  // 속도, HP, 공격력, 방어력, 저항
        CritChance, CritDamage, Penetration, Stun, Confusion, Evasion, // 크리확률, 크리뎀, 방어관통, 스턴확률, 혼란확률, 회피확률
        BaseAttack, Skill_1, Skill_2, Skill_3 // 행동
    }
    /// <summary>
    /// 변화가 증감인지 배수인지
    /// </summary>
    public enum SK_ChangeType
    {
        Modification, Multiplier
    }

    // 기본정보
    public readonly int skillCode;
    public readonly string name;
    public readonly string description;

    // 타입정보
    public readonly SK_Type sk_Type;
    public readonly SK_Attribute sK_Attribute;
    public readonly SK_ChangeType sK_ChangeType;
    // 애니메이션?

    // 세부정보
    public readonly int range; // 캐릭터부터 사용할 수 있는 최대 거리
    public readonly int impact; // 스킬의 강도.변화타입이 Multiplier 일땐 100을 나누어 변환 ex. 120이면 1.2 배

    /// <summary>
    /// 스킬 생성자
    /// </summary>
    /// <param name="skillCode">스킬 코드</param>
    /// <param name="name">스킬 이름</param>
    /// <param name="description">스킬 설명</param>
    /// <param name="sk_Type">스킬 타입</param>
    /// <param name="sK_Attribute">변화시킬 캐릭터의 속성</param>
    /// <param name="sK_ChangeType">변화 타입</param>
    /// <param name="range">스킬 범위</param>
    /// <param name="impact">스킬의 강도</param>
    public Skill(
        int skillCode,
        string name,
        string description,
        SK_Type sk_Type,
        SK_Attribute sK_Attribute,
        SK_ChangeType sK_ChangeType,
        int range,
        int impact
    )
    {
        this.skillCode = skillCode;
        this.name = name;
        this.description = description;
        this.sk_Type = sk_Type;
        this.sK_Attribute = sK_Attribute;
        this.sK_ChangeType = sK_ChangeType;
        this.range = range;
        this.impact = impact;
    }
}
public static class SkillDB
{
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
            return new Skill();
        }

        return SkillList[id];
    }
    public static void initializeSkillList()
    {
        if (SkillList.Count != 0) return;

        List<Dictionary<string, object>> dict = CSVReader.Read("Csvs/SkillInfo");
        foreach (Dictionary<string, object> item in dict)
        {
            int skillCode = (int)item["Skill_ID"];
            string name = (string)item["Name"];
            string description = (string)item["Description"];
            Skill.SK_Type type = Utility.StringToEnum<Skill.SK_Type>((string)item["Type"]);
            Skill.SK_Attribute attr = Utility.StringToEnum<Skill.SK_Attribute>((string)item["Attribute"]);
            Skill.SK_ChangeType cType = Utility.StringToEnum<Skill.SK_ChangeType>((string)item["ChangeType"]);
            int range = (int)item["Range"];
            int impact = (int)item["Impact"];
            Skill tempSkill = new Skill(skillCode, name, description, type, attr, cType, range, impact);
            SkillList.Add(tempSkill);
        }
    }
}

