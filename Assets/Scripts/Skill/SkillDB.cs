using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct Skill
{
    /// <summary>
    /// ��ų�� Ÿ��
    /// </summary>
    public enum SK_Type
    {
        Passive, Active, Buff, Special
    }
    /// <summary>
    /// ��ȭ��ų ĳ������ �Ӽ�
    /// </summary>
    public enum SK_Attribute
    {
        Speed, Health, Attack, Defence, Resist,  // �ӵ�, HP, ���ݷ�, ����, ����
        CritChance, CritDamage, Penetration, Stun, Confusion, Evasion, // ũ��Ȯ��, ũ����, ������, ����Ȯ��, ȥ��Ȯ��, ȸ��Ȯ��
        BaseAttack, Skill_1, Skill_2, Skill_3 // �ൿ
    }
    /// <summary>
    /// ��ȭ�� �������� �������
    /// </summary>
    public enum SK_ChangeType
    {
        Modification, Multiplier
    }

    // �⺻����
    public readonly int skillCode;
    public readonly string name;
    public readonly string description;

    // Ÿ������
    public readonly SK_Type sk_Type;
    public readonly SK_Attribute sK_Attribute;
    public readonly SK_ChangeType sK_ChangeType;
    // �ִϸ��̼�?

    // ��������
    public readonly int range; // ĳ���ͺ��� ����� �� �ִ� �ִ� �Ÿ�
    public readonly int impact; // ��ų�� ����.��ȭŸ���� Multiplier �϶� 100�� ������ ��ȯ ex. 120�̸� 1.2 ��

    /// <summary>
    /// ��ų ������
    /// </summary>
    /// <param name="skillCode">��ų �ڵ�</param>
    /// <param name="name">��ų �̸�</param>
    /// <param name="description">��ų ����</param>
    /// <param name="sk_Type">��ų Ÿ��</param>
    /// <param name="sK_Attribute">��ȭ��ų ĳ������ �Ӽ�</param>
    /// <param name="sK_ChangeType">��ȭ Ÿ��</param>
    /// <param name="range">��ų ����</param>
    /// <param name="impact">��ų�� ����</param>
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
    /// ��ų ID�� ���� ��ų ������ �������� �Լ�
    /// </summary>
    /// <param name="id">��ų ID</param>
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

