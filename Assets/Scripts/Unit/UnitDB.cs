using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public static class UnitDB 
{

    private static List<UnitBase> UnitList = new List<UnitBase>();
    /// <summary>
    /// 유닛 ID를 통해 유닛 정보를 가져오는 함수
    /// </summary>
    /// <param name="id">스킬 ID</param>
    /// <returns></returns>
    public static UnitBase GetSkill(int id)
    {
        if (UnitList.Count == 0)
        {
            initializeUnitList();
        }

        if (id >= UnitList.Count)
        {
            Debug.Log($"{id} is not Valid Unit ID");
            return null;
        }

        return new UnitBase(UnitList[id]);
    }
    public static void initializeUnitList()
    {
        if (UnitList.Count != 0) return;

        List<Dictionary<string, object>> dict = CSVReader.Read("Csvs/UnitInfo");
        foreach (Dictionary<string, object> item in dict)
        {
            int id = (int)item["Unit_ID"];
            string name = (string)item["Name"];
            Race race = Utility.StringToEnum<Race>((string)item["Race"]);
            Job job = Utility.StringToEnum<Job>((string)item["Job"]);
            Feature feat = Utility.StringToEnum<Feature>((string)item["Feature"]);
            int speed = getintValue("Speed");
            int HP = getintValue("HP");
            int atk = getintValue("Attack");
            int def = getintValue("Defence");
            int Res = getintValue("Resistance");
            int CritChance = getintValue("CritChance");
            int CritDamage = getintValue("CritDamage");
            int pene = getintValue("Penetration");
            int stun = getintValue("StunChance");
            int confu = getintValue("ConfusionChance");
            int dodge = getintValue("DodgeChance");
            Status status = new Status(HP,HP,atk,def,pene,Res,CritChance,CritDamage,stun,confu,dodge,speed);
            List<Skill> skills = new List<Skill>();
            for (int i = 0; i < 4; i++)
            {
                string temp = (string)item[$"Skill_Id_{i}"];
                if (temp == "-") continue;
                skills.Add(SkillDB.GetSkill(int.Parse(temp)));
            }

            UnitBase tempSkill = new UnitBase(id, name, status, job, feat, race, skills);
            UnitList.Add(tempSkill);

            int getintValue(string type)
            {
                return ((string)item["type"]).Contains('~') ? getRandomValue((string)item["type"]) : (int)item["type"];
            }
        }

    }

    private static int getRandomValue(string input)
    {
        int[] minMax = Array.ConvertAll(input.Split('~'), int.Parse);
        int answer = UnityEngine.Random.Range(minMax[0], minMax[1]);
        return answer;
    }
}
