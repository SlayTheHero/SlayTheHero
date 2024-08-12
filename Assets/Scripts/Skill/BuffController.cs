using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Init 한 뒤에 AddBuff를 이용하여 버프를 넣고
/// OnTurn 을 이용하여 버프의 시간을 제어합니다.
/// </summary>
public class BuffController
{
    /// <summary>
    /// Unit의 정보를 복사한 객체입니다.
    /// </summary>
    private UnitBase UnitSave;

    private UnitBase Unit;
    private Queue<Skill> BuffQueue;
    /// <summary>
    /// Unit을 등록합니다
    /// </summary>
    /// <param name="unit">해당 유닛 객체</param>
    public void Init(UnitBase unit)
    {
        Unit = unit;
        UnitSave = new UnitBase(unit);
    }
    /// <summary>
    /// 스킬에 맞는 상태를 등록한 Unit에 반영합니다
    /// 스킬은 DurationType이 Active이면 넣지 않습니다
    /// </summary>
    /// <param name="skill"></param>
    public void AddBuff(Skill skill)
    {
        if (skill.sk_DurationType == Skill.SK_DurationType.Active) return;
        switch(skill.sk_DurationType)
        {
            case Skill.SK_DurationType.Passive:
                AddPassive(skill);
                break;
            case Skill.SK_DurationType.Buff:
                AddQueue(skill);
                break;
            case Skill.SK_DurationType.Special:
                break;
        }
    }

    /// <summary>
    /// BuffQueue 내에 있는 버프의 NowDuration을 1씩 줄입니다.
    /// NowDuration이 0인 버프는 사라집니다.
    /// </summary>
    public void OnTurn()
    {
        int nCount = BuffQueue.Count;
        for (int i = 0; i < nCount; i++)
        {
            Skill skill = BuffQueue.Dequeue();
            if (skill.nowDuration == 1) continue;
            skill.nowDuration--;
            BuffQueue.Enqueue(skill);
        }
        ApplyBuff();
    }

    /// <summary>
    /// saveUnit에 반영합니다.
    /// </summary>
    /// <param name="skill"></param>
    private void AddPassive(Skill skill)
    {
        StatusController.ApplyAttribute(new Status(),UnitSave.Status,skill); 
        ApplyBuff();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skill"></param>
    private void AddQueue(Skill skill)
    {
        //같은 버프는? 순서는?
        BuffQueue.Enqueue(skill);
        ApplyBuff();
    }
    
    /// <summary>
    /// 큐에 있는 버프 중 앞에서부터 UnitSave에 적용한 결과를
    /// Unit의 Status에 반영합니다.
    /// Unit에 적용
    /// </summary>
    private void ApplyBuff()
    {
        int nCount = BuffQueue.Count;
        Status status = new Status(UnitSave.Status);
        for (int i = 0; i < nCount; i++)
        {
            Skill skill = BuffQueue.Dequeue();
            StatusController.ApplyAttribute(new Status(), status, skill);
            BuffQueue.Enqueue(skill);
        }
        int hp = Unit.Status.HP;
        Unit.Status = new Status(status);
        Unit.Status.HP = hp > Unit.Status.MaxHP ? hp : Unit.Status.MaxHP;
    }

}
