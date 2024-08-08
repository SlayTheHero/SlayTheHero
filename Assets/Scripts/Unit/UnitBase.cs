using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public int ID;
    public string Name;
    public Status Status;
    public Job Job;
    public Feature Feature;
    public Race Race;

    public List<Skill> skillList;
    public BuffController BuffController;
    //스킬 1 2 3 추가?

    // 디폴트 생성자
    public UnitBase()
    {
        skillList = new List<Skill>();
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
        skillList = new List<Skill>();
        for (int i = 0; i < skills.Count; i++)
        {
            skillList.Add(SkillDB.GetSkill(skills[i].id));
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
        skillList = new List<Skill>();
        for (int i = 0; i < other.skillList.Count; i++)
        {
            skillList.Add(other.skillList[i]);
        }
        BuffController = new BuffController();
    }

    /// <summary>
    /// 피격시 실행 크리티컬시 isCritical에 true
    /// </summary>
    /// <param name="endDamage">최종데미지</param>
    /// <param name="Attacker">공격자</param>
    /// <param name="isCritical">크리티컬여부</param>
    public void OnDamage(int endDamage,Status Attacker,bool isCritical)
    {
        int random = Random.Range(0,100) / 100;
        if(random < Status.DodgeChance)
        {
            //회피 효과
        }
        else 
        {
            // 크리티컬 효과
            if(isCritical) 
            {

            }
            else // 피격효과
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
