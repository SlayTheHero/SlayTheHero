using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Init �� �ڿ� AddBuff�� �̿��Ͽ� ������ �ְ�
/// OnTurn �� �̿��Ͽ� ������ �ð��� �����մϴ�.
/// </summary>
public class BuffController
{
    /// <summary>
    /// Unit�� ������ ������ ��ü�Դϴ�.
    /// </summary>
    private UnitBase UnitSave;

    private UnitBase Unit;
    private Queue<Skill> BuffQueue;
    /// <summary>
    /// Unit�� ����մϴ�
    /// </summary>
    /// <param name="unit">�ش� ���� ��ü</param>
    public void Init(UnitBase unit)
    {
        Unit = unit;
        UnitSave = new UnitBase(unit);
    }
    /// <summary>
    /// ��ų�� �´� ���¸� ����� Unit�� �ݿ��մϴ�
    /// ��ų�� DurationType�� Active�̸� ���� �ʽ��ϴ�
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
    /// BuffQueue ���� �ִ� ������ NowDuration�� 1�� ���Դϴ�.
    /// NowDuration�� 0�� ������ ������ϴ�.
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
    /// saveUnit�� �ݿ��մϴ�.
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
        //���� ������? ������?
        BuffQueue.Enqueue(skill);
        ApplyBuff();
    }
    
    /// <summary>
    /// ť�� �ִ� ���� �� �տ������� UnitSave�� ������ �����
    /// Unit�� Status�� �ݿ��մϴ�.
    /// Unit�� ����
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
