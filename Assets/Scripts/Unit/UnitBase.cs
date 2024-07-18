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

    public Skill Skill_0;
    public Skill Skill_1;
    public Skill Skill_2;
    public Skill Skill_3;

    public BuffController BuffController;
    //스킬 1 2 3 추가?

    // 디폴트 생성자
    public UnitBase() { }
    // 복사 생성자
    public UnitBase(UnitBase other)
    {
        ID = other.ID;
        Name = other.Name;
        Status = new Status(other.Status);
        Job = other.Job;
        Feature = other.Feature;
        Race = other.Race;
        Skill_0 = SkillDB.GetSkill(other.Skill_0.id);
        Skill_1 = SkillDB.GetSkill(other.Skill_1.id);
        Skill_2 = SkillDB.GetSkill(other.Skill_2.id);
        Skill_3 = SkillDB.GetSkill(other.Skill_3.id);
    }
}
