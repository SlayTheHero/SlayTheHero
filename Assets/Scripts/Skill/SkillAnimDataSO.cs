using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewSklilAnimDataSO", menuName = "ScriptableObject/SkillAnimData")]
public class SkillAnimDataSO : ScriptableObject
{
    public int SkillID;
    [Serializable]
    public class Anim
    {
        public AnimType Type;
        public string Name;
        public int StartDelay;
        public int Layer;
        public AnimationClip Clip;
        public bool IsLoop;
        public bool IsAutoReleased;
        public Vector2 StartLocalPos;
        public Anim(AnimType type, string name, int startDelay,int layer, AnimationClip clip, bool isLoop, Vector2 startLocalPos)
        {
            Type = type;
            Name = name;
            StartDelay = startDelay;
            Layer = layer;
            Clip = clip;
            IsLoop = isLoop;
            StartLocalPos = startLocalPos;
        }
    }
    [Serializable]
    public enum AnimType
    {
        UnitMotion, UnitEffect, HitEffect, TargetedEffect, ProjectileFly, ProjectileEffect
    }
    public Anim[] UnitMotions;
    public Anim[] UnitEffects;
    public Anim[] HitEffects;
    public Anim[] TargetedEffects;
    public Anim[] ProjectileFlys;
    public Anim[] ProjectileEffects;
    public TrajectoryDataSO TrajectoryData;

    private List<Anim> allAnim;
    public List<Anim> AllAnim
    {
        get
        {
            if (allAnim == null)
            {
                var ret = new List<Anim>();
                ret.AddRange(UnitMotions);
                ret.AddRange(UnitEffects);
                ret.AddRange(HitEffects);
                ret.AddRange(TargetedEffects);
                ret.AddRange(ProjectileFlys);
                ret.AddRange(ProjectileEffects);
                allAnim = ret;
            }
            return allAnim;
        }
    }
}
