using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UnitDB에서 물결표시에
/// </summary>
public class RandomStatus : Status
{
    public RandomStatus()
    {

    }
    public RandomStatus(
        (int, int) range_hp,
        (int, int) range_maxHP,
        (int, int) range_atk,
        (int, int) range_def,
        (int, int) range_pen,
        (int, int) range_res,
        (int, int) range_criticalChance,
        (int, int) range_criticalDamage,
        (int, int) range_stun,
        (int, int) range_confusion,
        (int, int) range_dodge,
        (int, int) range_speed)
    {
        Range_hp = range_hp;
        Range_maxHP = range_maxHP;
        Range_atk = range_atk;
        Range_def = range_def;
        Range_pen = range_pen;
        Range_res = range_res;
        Range_criticalChance = range_criticalChance;
        Range_criticalDamage = range_criticalDamage;
        Range_stun = range_stun;
        Range_confusion = range_confusion;
        Range_dodge = range_dodge;
        Range_speed = range_speed;
    }
    (int,int) Range_hp;
    (int,int) Range_maxHP;
    (int,int) Range_atk;
    (int,int) Range_def;
    (int,int) Range_pen;
    (int,int) Range_res;
    (int,int) Range_criticalChance;
    (int,int) Range_criticalDamage;
    (int,int) Range_stun;
    (int,int) Range_confusion;
    (int,int) Range_dodge;
    (int,int) Range_speed;

    public void setRandomValue()
    {
        HP = getValueForRange(Range_hp);
        MaxHP = getValueForRange(Range_maxHP);
        ATK = getValueForRange(Range_atk);
        DEF = getValueForRange(Range_def);
        Penetration = getValueForRange(Range_pen);
        Resistance = getValueForRange(Range_res);
        CriticalChance = getValueForRange(Range_criticalChance);
        CriticalDamage = getValueForRange(Range_criticalDamage);
        StunChance = getValueForRange(Range_stun);
        ConfusionChance = getValueForRange(Range_confusion);
        DodgeChance = getValueForRange(Range_dodge);
        Speed = getValueForRange(Range_speed);
    }

    private int getValueForRange((int,int) range)
    {
        if(range.Item1 == range.Item2) return range.Item1;
        return UnityEngine.Random.Range(range.Item1, range.Item2 + 1);
    }

}
