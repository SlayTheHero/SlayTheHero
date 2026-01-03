using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static SkillAnimDataSO;
using static UnityEngine.GraphicsBuffer;

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
        var skill_data = skill.SkillAnimData;
        var attacker_obj = BattleManager.Instance.Units[Attacker.Position];
        var target_obj = BattleManager.Instance.Units[Target.Position];
        attacker_obj.transform.DOLocalMoveX(Attacker.IsPlayerUnit ? 0.5f : -0.5f, 1).SetRelative();
        attacker_obj.transform.DOScale(2.3f, 1);
        await RunAllSkillAnimAsync(skill_data, attacker_obj, target_obj);
        attacker_obj.transform.DOLocalMoveX(Attacker.IsPlayerUnit ? -0.5f : 0.5f, 1).SetRelative();
        attacker_obj.transform.DOScale(2.0f, 1);
        await Task.Delay(1500);
    }
    private static async Task ProjectileBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        var skill_data = skill.SkillAnimData;
        var attacker_obj = BattleManager.Instance.Units[Attacker.Position];
        var target_obj = BattleManager.Instance.Units[Target.Position];

        await RunAllSkillAnimAsync(skill_data, attacker_obj, target_obj);
    }
    private static async Task BuffBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        var skill_data = skill.SkillAnimData;
        var attacker_obj = BattleManager.Instance.Units[Attacker.Position];
        var target_obj = BattleManager.Instance.Units[Target.Position];

        await RunAllSkillAnimAsync(skill_data, attacker_obj, target_obj);
    }

    private static Dictionary<int, SkillDelegate> SpecialSkillDict = new Dictionary<int, SkillDelegate>();
    private static async Task SpecialBehavior(UnitBase Attacker, UnitBase Target, Skill skill)
    {
        switch (skill.id)
        {
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

    private static async Task RunAllSkillAnimAsync(SkillAnimDataSO skill_data, GameObject attacker_obj, GameObject target_obj)
    {
        bool has_hit = false;
        int progress_time = 0;
        var attacker_anim = attacker_obj.GetComponent<Animator>();
        var target_anim = target_obj.GetComponent<Animator>();
        skill_data.AllAnim.Sort((a, b) => a.StartDelay.CompareTo(b.StartDelay));
        foreach (var skillAnim in skill_data.AllAnim)
        {
            if (skillAnim.StartDelay > progress_time)
            {
                int d = skillAnim.StartDelay - progress_time;
                await Task.Delay(d);
                progress_time += d;
            }
            switch (skillAnim.Type)
            {
                case AnimType.UnitMotion:
                    attacker_anim.SetTrigger(skillAnim.Name);
                    break;
                case AnimType.UnitEffect:
                    if (!skillAnim.IsParticle)
                        EffectManager.Instance.CreateEffect(skillAnim.Clip, attacker_obj.transform.position + (Vector3)skillAnim.StartLocalPos, skillAnim.Layer, skillAnim.IsAutoReleased).Invoke();
                    EffectManager.Instance.CreateParticle(skillAnim.Name);
                    break;
                case AnimType.HitEffect:
                    if (!skillAnim.IsParticle)
                        EffectManager.Instance.CreateEffect(skillAnim.Clip, target_obj.transform.position + (Vector3)skillAnim.StartLocalPos, skillAnim.Layer, skillAnim.IsAutoReleased).Invoke();
                    else
                        EffectManager.Instance.CreateParticle(skillAnim.Name);
                    if (!has_hit) { target_anim.SetTrigger("Hit"); has_hit = true; };
                    break;
                case AnimType.TargetedEffect:
                    if (!skillAnim.IsParticle)
                        EffectManager.Instance.CreateEffect(skillAnim.Clip, target_obj.transform.position + (Vector3)skillAnim.StartLocalPos, skillAnim.Layer, skillAnim.IsAutoReleased).Invoke();
                    EffectManager.Instance.CreateParticle(skillAnim.Name);
                    break;
                case AnimType.ProjectileFly:
                    var obj = Projectile.Pool.Get();
                    var proj = obj.GetComponent<Projectile>();
                    proj.transform.position = attacker_obj.transform.position;
                    proj.Init(skill_data.TrajectoryData, target_obj.transform.position + (Vector3)skillAnim.StartLocalPos);
                    proj.Shoot();
                    break;
                default: break;

            }
        }
    }


}
