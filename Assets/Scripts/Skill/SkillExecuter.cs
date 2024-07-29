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
        if (skill.sk_DurationType == Skill.SK_DurationType.Passive) return;

        //행동
        RunBehaviorLogic(Attacker, Target, skill);
        //지속시간
        switch(skill.sk_DurationType)
        {
            case Skill.SK_DurationType.Active:
                StatusController.ApplyAttribute(Attacker.Status, Target.Status, skill);
                break;
            case Skill.SK_DurationType.Passive:
                ApplyBuff(Attacker, Target, skill);
                break;
            case Skill.SK_DurationType.Buff:
                ApplyBuff(Attacker, Target, skill);
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


    #region ApplyBuff
    private static void ApplyBuff(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        Target.BuffController.AddBuff(skill);
    }
    #endregion ApplyBuff


}
