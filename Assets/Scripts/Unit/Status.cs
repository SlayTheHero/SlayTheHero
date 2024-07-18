using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
[Serializable]
public class Status
{
    public event Action<string,double> OnStatusChange;
    double hp;
    double maxHP;
    double atk;
    double def;
    double pen;
    double res;
    double criticalChance;
    double criticalDamage;
    double stun;
    double confusion;
    double dodge;
    double speed;

    public double HP { get { return hp; } set { hp = value; OnStatusChange.Invoke("HP", value); } }
    public double MaxHP { get { return maxHP; } set { maxHP = value; OnStatusChange.Invoke("MaxHP", value); } }
    public double ATK { get { return atk; } set { atk = value; OnStatusChange.Invoke("ATK", value); } }
    public double DEF { get { return def; } set { def = value; OnStatusChange.Invoke("DEF", value); } }
    public double Penetration { get { return pen; } set { pen = value; OnStatusChange.Invoke("Penetration", value); } }
    public double Resistance { get { return res; } set { res = value; OnStatusChange.Invoke("Resistance", value); } }
    public double CriticalChance { get { return criticalChance; } set { criticalChance = value; OnStatusChange.Invoke("CriticalChance", value); } }
    public double CriticalDamage { get { return criticalDamage; } set { criticalDamage = value; OnStatusChange.Invoke("CriticalDamage", value); } }
    public double StunChance { get { return stun; } set { stun = value; OnStatusChange.Invoke("StunChance", value); } }
    public double ConfusionChance { get { return confusion; } set { confusion = value; OnStatusChange.Invoke("ConfusionChance", value); } }
    public double DodgeChance { get { return dodge; } set { dodge = value; OnStatusChange.Invoke("DodgeChance", value); } }
    public double Speed { get { return speed; } set { speed = value; OnStatusChange.Invoke("Speed",value); } }

    public static Status operator +(Status left, Status right)
    {
        Status ret = new()
        {
            HP = left.HP + right.HP,
            MaxHP = left.MaxHP + right.MaxHP,
            ATK = left.ATK + right.ATK,
            DEF = left.DEF + right.DEF,
            Penetration = left.Penetration + right.Penetration,
            Resistance = left.Resistance + right.Resistance,
            CriticalChance = left.CriticalChance + right.CriticalChance,
            CriticalDamage = left.CriticalDamage + right.CriticalDamage,
            StunChance = left.StunChance + right.StunChance,
            ConfusionChance = left.ConfusionChance + right.ConfusionChance,
            DodgeChance = left.DodgeChance + right.DodgeChance,
            Speed = left.Speed + right.Speed,
        };
        return ret;
    }

    // 디폴트 생성자
    public Status() {}

    // 복사 생성자
    public Status(Status other)
    {
        HP = other.HP; 
        MaxHP = other.MaxHP;
        ATK = other.ATK;
        DEF = other.DEF;
        Penetration = other.Penetration;
        Resistance = other.Resistance;
        CriticalChance = other.CriticalChance;
        CriticalDamage = other.CriticalDamage;
        ConfusionChance = other.ConfusionChance;
        DodgeChance = other.DodgeChance;
    }
}
