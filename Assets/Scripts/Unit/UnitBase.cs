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


    // ï¿½ï¿½ï¿½ï¿½Æ® ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    public UnitBase()
    {
        SkillList = new List<Skill>();
        BuffController = new BuffController();
    }
    // ï¿½Ä¶ï¿½ï¿½ï¿½ï¿?ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
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


    // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
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
    /// ï¿½Ç°Ý½ï¿½ ï¿½ï¿½ï¿½ï¿½ Å©ï¿½ï¿½Æ¼ï¿½Ã½ï¿½ isCriticalï¿½ï¿½ true
    /// </summary>
    /// <param name="endDamage">ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½</param>
    /// <param name="Attacker">ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½</param>
    /// <param name="isCritical">Å©ï¿½ï¿½Æ¼ï¿½Ã¿ï¿½ï¿½ï¿½</param>
    public void OnDamage(int endDamage,Status Attacker,bool isCritical)
    {
        int random = Random.Range(0,100) / 100;
        if(random < Status.DodgeChance)
        {
            //È¸ï¿½ï¿½ È¿ï¿½ï¿½
        }
        else 
        {
            // Å©ï¿½ï¿½Æ¼ï¿½ï¿½ È¿ï¿½ï¿½
            if(isCritical) 
            {

            }
            else // ï¿½Ç°ï¿½È¿ï¿½ï¿½
            {

            }

            Status.OnDamage(endDamage, Attacker);
            
            // »ç¸Á ·ÎÁ÷ StatusÂÊ¿¡¼­ ÇØµµ µÉÁöµµ
            if(Status.HP <= 0)
            {

            }
        }
    }

}
