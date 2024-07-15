using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;

public static class SkillExecuter
{
    delegate void SkillDelegate(UnitBase Attacker, UnitBase Target, Skill skill);
    public static void Execute(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        //행동
        RunBehaviorLogic(Attacker, Target, skill);
        //지속시간
        switch(skill.sk_DurationType)
        {
            case Skill.SK_DurationType.Active:
                ApplyAttribute(Attacker, Target, skill);
                break;
            case Skill.SK_DurationType.Passive:
                ApplyBuff(Attacker, Target, skill,-1);
                break;
            case Skill.SK_DurationType.Buff:
                ApplyBuff(Attacker, Target, skill, skill.duration);
                break;
            case Skill.SK_DurationType.Special:
                break;
        }
    }

    #region Behaviors

    private static void RunBehaviorLogic(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        switch (skill.sK_BehaviorType)
        {
            case Skill.SK_BehaviorType.Melee:
                MeleeBehavior(Attacker, Target, skill);
                break;
            case Skill.SK_BehaviorType.Projectile:
                ProjectileBehavior(Attacker, Target, skill);
                break;
            case Skill.SK_BehaviorType.Buff:
                BuffBehavior(Attacker, Target, skill);
                break;
            case Skill.SK_BehaviorType.Special:
                SpecialBehavior(Attacker, Target, skill);
                break;
        }
    }
    private static void MeleeBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {

    }
    private static void ProjectileBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {

    }
    private static void BuffBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {

    }

    private static Dictionary<int,SkillDelegate> SpecialSkillDict = new Dictionary<int,SkillDelegate>();
    private static void SpecialBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        //스킬ID로 파싱 후 특수로직 따로 작성
    }

    #endregion Behaviors

    #region ApplyAttribute
    private delegate int operatorDelegate(double attribute, int impact);
    private static void ApplyAttribute(UnitBase Attacker, UnitBase Target, Skill skill)
    {
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
        double attributeValue = 0;

        switch (skill.sK_Attribute)
        {
            case Skill.SK_Attribute.Speed:
                // attributeValue = Target.Speed 속도 x
                break;
            case Skill.SK_Attribute.Health:
                attributeValue = Target.Status.HP;
                break;
            case Skill.SK_Attribute.Attack:
                attributeValue = Target.Status.ATK;
                break;
            case Skill.SK_Attribute.Defence:
                attributeValue = Target.Status.DEF;
                break;
            case Skill.SK_Attribute.Resist:
                attributeValue = Target.Status.Resistance;
                break;
            case Skill.SK_Attribute.CritChance:
                attributeValue = Target.Status.CriticalChance;
                break;
            case Skill.SK_Attribute.CritDamage:
                attributeValue = Target.Status.CriticalDamage;
                break;
            case Skill.SK_Attribute.Penetration:
                attributeValue = Target.Status.Penetration;
                break;
            case Skill.SK_Attribute.Stun:
                attributeValue = Target.Status.StunChance;
                break;
            case Skill.SK_Attribute.Confusion:
                attributeValue = Target.Status.ConfusionChance;
                break;
            case Skill.SK_Attribute.Dodge:
                attributeValue = Target.Status.DodgeChance;
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
        nowOperator.Invoke(attributeValue, impactValue);
        switch (skill.sK_Attribute)
        {
            case Skill.SK_Attribute.Speed:
                //  Target.Speed  = attributeValue 속도 x
                break;
            case Skill.SK_Attribute.Health:
                Target.Status.HP = attributeValue;
                break;
            case Skill.SK_Attribute.Attack:
                Target.Status.ATK = attributeValue;
                break;
            case Skill.SK_Attribute.Defence:
                Target.Status.DEF = attributeValue;
                break;
            case Skill.SK_Attribute.Resist:
                Target.Status.Resistance = attributeValue;
                break;
            case Skill.SK_Attribute.CritChance:
                Target.Status.CriticalChance = attributeValue;
                break;
            case Skill.SK_Attribute.CritDamage:
                Target.Status.CriticalDamage = attributeValue;
                break;
            case Skill.SK_Attribute.Penetration:
                Target.Status.Penetration = attributeValue;
                break;
            case Skill.SK_Attribute.Stun:
                Target.Status.StunChance = attributeValue;
                break;
            case Skill.SK_Attribute.Confusion:
                Target.Status.ConfusionChance = attributeValue;
                break;
            case Skill.SK_Attribute.Dodge:
                Target.Status.DodgeChance = attributeValue;
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
    #endregion ApplyAttribute

    #region ApplyBuff
    private static void ApplyBuff(UnitBase Attacker, UnitBase Target, Skill skill,int duration)
    {

    }
    #endregion ApplyBuff


}
