using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusController
{
    private delegate int operatorDelegate(double attribute, int impact);
    private static UnitBase unit = new UnitBase();
    public static void ApplyAttribute(UnitBase AttackerUnit, UnitBase targetUnit, Skill skill)
    {
        Status Attacker = AttackerUnit.Status;
        Status target = targetUnit.Status;

        operatorDelegate nowOperator;
        switch (skill.sK_ChangeType)
        {
            case Skill.SK_ChangeType.Modification:
                nowOperator = Modificate;
                break;
            case Skill.SK_ChangeType.Multiplication:
                nowOperator = Multiplicate;
                break;
            default:
                nowOperator = null;
                break;
        }
        int impactValue = skill.impact;
        int attributeValue = 0;
        
        if(skill.sK_Attribute == Skill.SK_Attribute.Health)
        {
            int random = Random.Range(0, 100) / 100;
            bool isCritical = false;
            float critical = 1;
            if(Attacker.CriticalChance < random)
            {
                isCritical = true; critical = 1 + ((float)Attacker.CriticalDamage / 100);
            }

            if (impactValue == 0)
            {
                impactValue = Attacker.ATK;
            }
            else
            {
                impactValue = skill.impact;
            }

            float endDamage = impactValue * critical;
            targetUnit.OnDamage((int)endDamage, Attacker, isCritical);

        }

        switch (skill.sK_Attribute)
        {
            case Skill.SK_Attribute.Speed:
                attributeValue = target.Speed;
                break;
            case Skill.SK_Attribute.Attack:
                attributeValue = target.ATK;
                break;
            case Skill.SK_Attribute.Defence:
                attributeValue = target.DEF;
                break;
            case Skill.SK_Attribute.Resist:
                attributeValue = target.Resistance;
                break;
            case Skill.SK_Attribute.CritChance:
                attributeValue = target.CriticalChance;
                break;
            case Skill.SK_Attribute.CritDamage:
                attributeValue = target.CriticalDamage;
                break;
            case Skill.SK_Attribute.Penetration:
                attributeValue = target.Penetration;
                break;
            case Skill.SK_Attribute.StunChance:
                attributeValue = target.StunChance;
                break;
            case Skill.SK_Attribute.ConfusionChance:
                attributeValue = target.ConfusionChance;
                break;
            case Skill.SK_Attribute.Dodge:
                attributeValue = target.DodgeChance;
                break;
            case Skill.SK_Attribute.BaseAttack:
                break;
            case Skill.SK_Attribute.Skill_1:
                break;
            case Skill.SK_Attribute.Skill_2:
                break;
            case Skill.SK_Attribute.Skill_3:
                break;
            case Skill.SK_Attribute.Stun:
                attributeValue = target.StunChance;
                break;
            case Skill.SK_Attribute.Confusion:
                attributeValue = target.ConfusionChance;
                break;
        }
        nowOperator.Invoke(attributeValue, impactValue);
        switch (skill.sK_Attribute)
        {
            case Skill.SK_Attribute.Speed:
                target.Speed = attributeValue;
                break;
            case Skill.SK_Attribute.Attack:
                target.ATK = attributeValue;
                break;
            case Skill.SK_Attribute.Defence:
                target.DEF = attributeValue;
                break;
            case Skill.SK_Attribute.Resist:
                target.Resistance = attributeValue;
                break;
            case Skill.SK_Attribute.CritChance:
                target.CriticalChance = attributeValue;
                break;
            case Skill.SK_Attribute.CritDamage:
                target.CriticalDamage = attributeValue;
                break;
            case Skill.SK_Attribute.Penetration:
                target.Penetration = attributeValue;
                break;
            case Skill.SK_Attribute.Stun:
                target.StunChance = attributeValue;
                break;
            case Skill.SK_Attribute.Confusion:
                target.ConfusionChance = attributeValue;
                break;
            case Skill.SK_Attribute.Dodge:
                target.DodgeChance = attributeValue;
                break;
            case Skill.SK_Attribute.BaseAttack:
                break;
            case Skill.SK_Attribute.Skill_1:
                break;
            case Skill.SK_Attribute.Skill_2:
                break;
            case Skill.SK_Attribute.Skill_3:
                break;
        }

    }
    private static int Modificate(double attribute, int impact)
    {
        return (int)(attribute + impact);
    }
    private static int Multiplicate(double attribute, int impact)
    {
        return (int)(attribute * (1 + ((double)impact / 100.0)));
    }

}
