using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
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
    
    private void Awake()
    {
        ID = 0;
        Status = new Status();
        SkillList = new List<Skill>();
    }
}
