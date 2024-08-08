using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
[Serializable]
public class Status
{
    public event Action<string, int> OnStatusChange;
    int hp;
    int maxHP;
    int atk;
    int def;
    int pen;
    int res;
    int criticalChance;
    int criticalDamage;
    int stun;
    int confusion;
    int dodge;
    int speed;

    public int HP { get { return hp; } set { hp = value; OnStatusChange.Invoke("HP", value); } }
    public int MaxHP { get { return maxHP; } set { maxHP = value; OnStatusChange.Invoke("MaxHP", value); } }
    public int ATK { get { return atk; } set { atk = value; OnStatusChange.Invoke("ATK", value); } }
    public int DEF { get { return def; } set { def = value; OnStatusChange.Invoke("DEF", value); } }
    public int Penetration { get { return pen; } set { pen = value; OnStatusChange.Invoke("Penetration", value); } }
    public int Resistance { get { return res; } set { res = value; OnStatusChange.Invoke("Resistance", value); } }
    public int CriticalChance { get { return criticalChance; } set { criticalChance = value; OnStatusChange.Invoke("CriticalChance", value); } }
    public int CriticalDamage { get { return criticalDamage; } set { criticalDamage = value; OnStatusChange.Invoke("CriticalDamage", value); } }
    public int StunChance { get { return stun; } set { stun = value; OnStatusChange.Invoke("StunChance", value); } }
    public int ConfusionChance { get { return confusion; } set { confusion = value; OnStatusChange.Invoke("ConfusionChance", value); } }
    public int DodgeChance { get { return dodge; } set { dodge = value; OnStatusChange.Invoke("DodgeChance", value); } }
    public int Speed { get { return speed; } set { speed = value; OnStatusChange.Invoke("Speed", value); } }

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
    public Status() { }

    // 파라미터 생성자
    // int 파라미터 받는 생성자 추가
    public Status(int hp, int maxHP, int atk, int def, int penetration, int resistance, int criticalChance, int criticalDamage, int stunChance, int confusionChance, int dodgeChance, int speed)
    {
        HP = hp;
        MaxHP = maxHP;
        ATK = atk;
        DEF = def;
        Penetration = penetration;
        Resistance = resistance;
        CriticalChance = criticalChance;
        CriticalDamage = criticalDamage;
        StunChance = stunChance;
        ConfusionChance = confusionChance;
        DodgeChance = dodgeChance;
        Speed = speed;
    }

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
        StunChance = other.StunChance;
        ConfusionChance = other.ConfusionChance;
        DodgeChance = other.DodgeChance;
        Speed = other.Speed;
    }
    /// <summary>
    /// Status용 체력변화
    /// </summary>
    /// <param name="endDamage">최종데미지</param>
    /// <param name="Attacker">공격자</param>
    public void OnDamage(int endDamage, Status Attacker)
    {
        int damage = (int)((double)endDamage * (double)(100 - DEF + Attacker.Penetration) / 100.0);
        HP -= damage;
    }
}

