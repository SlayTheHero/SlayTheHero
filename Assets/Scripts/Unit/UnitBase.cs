using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class UnitBase : ISerializableToCSV
{
    public bool IsPlayerUnit;
    public int ID;
    public string Name;
    [SerializeField]
    public Status Status;
    public Job Job;
    public Feature Feature;
    public Race Race;
    public List<Skill> SkillList;
    public int Turn;
    public BuffController BuffController;
    public SynergyController SynergyController;


    // 기본생성자
    public UnitBase()
    {
        SkillList = new List<Skill>();
        BuffController = new BuffController();
    }
    // 파라미터 생성자
    public UnitBase(int id, string name, Status status, Job job, Feature feature, Race race, List<Skill> skills)
    {
        ID = id;
        Name = name;
        Status = status;
        Job = job;
        Feature = feature;
        Race = race;
        SkillList = new List<Skill>();
        for (int i = 0; i < skills.Count; i++)
        {
            SkillList.Add(SkillDB.GetSkill(skills[i].id));
        }
        BuffController = new BuffController();
    }


    // 복사 생성자
    public UnitBase(UnitBase other)
    {
        ID = other.ID;
        Name = other.Name;
        Status = new Status(other.Status);
        Job = other.Job;
        Feature = other.Feature;
        Race = other.Race;
        SkillList = new List<Skill>();
        for (int i = 0; i < other.SkillList.Count; i++)
        {
            SkillList.Add(other.SkillList[i]);
        }
        BuffController = new BuffController();
    }

    /// <summary>
    /// 데미지 피격시 호출할 함수
    /// </summary>
    /// <param name="endDamage">최종 데미지</param>
    /// <param name="Attacker">공격자</param>
    /// <param name="isCritical">크리티컬 여부</param>
    public void OnDamage(int endDamage,Status Attacker,bool isCritical)
    {
        int random = Random.Range(0,100) / 100;
        if(random < Status.DodgeChance)
        {
            // 회피 로직
        }
        else 
        {
            // 크리 로직
            if(isCritical) 
            {

            }
            else 
            {

            }

            Status.OnDamage(endDamage, Attacker);
            
            // 사망 로직 Status쪽에서 해도 될지도
            if(Status.HP <= 0)
            {

            }
        }
    }

    public string ToCSV()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("UnitBase").Append(",");
        sb.Append(ID).Append(",");
        sb.Append(Name).Append(",");
        sb.Append((int)Job).Append(",");
        sb.Append((int)Feature).Append(",");
        sb.Append((int)Race).Append(",");
        for (int i = 0; i < 4; i++)
        {
            if(i < SkillList.Count - 1)
            {
                sb.Append(SkillList[i].id).Append(",");
            }
            else
            {
                sb.Append(-1).Append(",");
            }
        }
        sb.Append("%");
        sb.Append(Status.ToCSV());

        return sb.ToString();
    }

    public void FromCSV(string data)
    {
        string[] DataArr = data.Split('%');
        string[] UnitBaseData = DataArr[0].Split(',');
        string statusData = DataArr[1];
        if (UnitBaseData[0] != "UnitBase")
        {
            Debug.Log($"{data} is not Valid UnitBase");
            return;
        }
        ID = int.Parse(UnitBaseData[1]);
        Name = UnitBaseData[2];
        Job = (Job)int.Parse(UnitBaseData[3]);
        Feature = (Feature)int.Parse(UnitBaseData[4]);
        Race = (Race)int.Parse(UnitBaseData[5]);
        for (int i = 6; i < 10; i++)
        {
            int skillID = int.Parse(UnitBaseData[i]);
            if (skillID == -1)
            {
                continue;
            }

            SkillList.Add(SkillDB.GetSkill(skillID));
        }

        Status = new Status();
        Status.FromCSV(statusData);
    }
}
