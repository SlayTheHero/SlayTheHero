using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Skill
{
    /// <summary>
    /// ��ų�� �ۿ뿡 ���� Ÿ��
    /// </summary>
    public enum SK_BehaviorType
    {
        Melee,           // ex. �̵�
        Projectile,      // ex. ����ü
        Buff,             // ex. ����ü ���� Ÿ�ٿ��� Ư�� �ִϸ��̼�
        Special           // Ư��
    }

    /// <summary>
    /// ��ų�� ���ӽð��� ���� Ÿ��
    /// </summary>
    public enum SK_DurationType
    {
        Passive, Active, Buff, Special
    }
    /// <summary>
    /// ��ȭ��ų ĳ������ �Ӽ�
    /// </summary>
    public enum SK_Attribute
    {
        Speed, MaxHealth ,Health, Attack, Defence, Resist,  // �ӵ�, MaxHP ,HP, ���ݷ�, ����, ����
        CritChance, CritDamage, Penetration, StunChance, ConfusionChance, Dodge, // ũ��Ȯ��, ũ����, ������, ����Ȯ��, ȥ��Ȯ��, ȸ��Ȯ��
        BaseAttack, Skill_1, Skill_2, Skill_3, // �ൿ
        Stun, Confusion
    }
    /// <summary>
    /// ��ȭ�� �������� �������
    /// </summary>
    public enum SK_ChangeType
    {
        Modification, Multiplication
    }

    // �⺻����
    public int id;
    public string name;
    public string description;

    // Ÿ������
    public SK_BehaviorType sK_BehaviorType;
    public SK_DurationType sk_DurationType;
    public SK_Attribute sK_Attribute;
    public SK_ChangeType sK_ChangeType;

    // �ִϸ��̼�?

    // ��������
    public int range; // ĳ���ͺ��� ����� �� �ִ� �ִ� �Ÿ�
    public int impact; // ��ų�� ����.��ȭŸ���� Multiplier �϶� 100�� ������ ��ȯ ex. 120�̸� 1.2 ��
    public int duration; //  ��ų ���ӽð� 
    public int coolTime; //  ��ų ��Ÿ��
    public int nowCoolDown = 0; // ���� ��Ÿ��
    public int nowDuration = 0; // ���� ���ӽð�




    /// <summary>
    /// ��ų ������
    /// </summary>
    /// <param name="id">��ų �ڵ�</param>
    /// <param name="name">��ų �̸�</param>
    /// <param name="description">��ų ����</param>
    /// <param name="sK_BehaviorType">��ų �ൿ Ÿ��</param>
    /// <param name="sk_DurationType">��ų ���ӽð� Ÿ��</param>
    /// <param name="sK_Attribute">��ȭ��ų ĳ������ �Ӽ�</param>
    /// <param name="sK_ChangeType">��ȭ Ÿ��</param>
    /// <param name="range">��ų ����</param>
    /// <param name="impact">��ų ����</param>
    /// <param name="duration">��ų ���ӽð�</param>
    /// <param name="coolTime">��ų ��Ÿ��</param>
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
    // ���� ������
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
    /// ������ Ÿ�Ժ��� ��ų ������ �Ҵ�
    /// </summary>
    public static Dictionary<string, List<int>> SkillTypeData = new Dictionary<string, List<int>>();
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

