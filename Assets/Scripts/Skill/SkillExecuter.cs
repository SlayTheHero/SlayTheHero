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
    public static async void Execute(UnitBase Attacker, UnitBase Target, Skill skill)
    {

        //지속시간
        switch (skill.sk_DurationType)
        {
            case Skill.SK_DurationType.Active:
                StatusController.ApplyAttribute(Attacker, Target, skill);
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
        //행동
        await RunBehaviorLogic(Attacker, Target, skill);

        Debug.Log("Skill Use finished");
        //OnSkillUsed 삽입

    }

    #region Behaviors

    private static async Task RunBehaviorLogic(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        switch (skill.sK_BehaviorType)
        {
            case Skill.SK_BehaviorType.Melee:
                await MeleeBehavior(Attacker, Target, skill);
                break;
            case Skill.SK_BehaviorType.Projectile:
                await ProjectileBehavior(Attacker, Target, skill);
                break;
            case Skill.SK_BehaviorType.Buff:
                await BuffBehavior(Attacker, Target, skill);
                break;
            case Skill.SK_BehaviorType.Special:
                await SpecialBehavior(Attacker, Target, skill);
                break;
        }
    }
    private static async Task MeleeBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        int index = 0;
        while(index < 5)
        {
            Debug.Log($"{index} Count");
            index++;
            await Task.Delay(1000);
        }
    }
    private static async Task ProjectileBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {

    }
    private static async Task BuffBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {

    }

    private static Dictionary<int, SkillDelegate> SpecialSkillDict = new Dictionary<int, SkillDelegate>();
    private static async Task SpecialBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        switch (skill.id)
        {
            case -1:
                Target.OnDamage(10, Attacker.Status, false);
                BattleManager.Instance.OnSkillUsed.Invoke();
                break;
            default: break;
        }
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
