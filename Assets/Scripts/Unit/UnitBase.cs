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
    //��ų 1 2 3 �߰�?

    // ����Ʈ ������
    public UnitBase()
    {
        SkillList = new List<Skill>();
        BuffController = new BuffController();
    }
    // �Ķ���� ������
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


    // ���� ������
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
    /// �ǰݽ� ���� ũ��Ƽ�ý� isCritical�� true
    /// </summary>
    /// <param name="endDamage">����������</param>
    /// <param name="Attacker">������</param>
    /// <param name="isCritical">ũ��Ƽ�ÿ���</param>
    public void OnDamage(int endDamage,Status Attacker,bool isCritical)
    {
        int random = Random.Range(0,100) / 100;
        if(random < Status.DodgeChance)
        {
            //ȸ�� ȿ��
        }
        else 
        {
            // ũ��Ƽ�� ȿ��
            if(isCritical) 
            {

            }
            else // �ǰ�ȿ��
            {

            }

            int damage = (int)((double)endDamage * (double)(100 - Status.DEF + Attacker.Penetration) / 100.0);
            
            // ��� ����
            if(Status.HP <= damage)
            {

            }else
            {
                Status.HP -= damage;
            }
        }
    }

}
