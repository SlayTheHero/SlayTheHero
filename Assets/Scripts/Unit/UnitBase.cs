using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitBase
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

}
