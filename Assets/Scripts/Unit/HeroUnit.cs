using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HeroUnit : UnitBase
{
    public HeroUnit() : base()
    {
        IsPlayerUnit = false;
    }
    public HeroUnit(UnitBase unitBase) : base(unitBase)
    {

        IsPlayerUnit = false;
    }
    public void Behave()
    {
        //보유 스킬중 랜덤으로 사용
        //사용 범위 고려
        for (int i = 0; i < SkillList.Count; i++)
        {
            var skill = SkillList[i];
            if (HasUnitInSkillRange(skill))
            {
                BattleManager.Instance.SelectedSkillNum = i;
                BattleManager.Instance.SkillUsed.Invoke(BattleManager.Instance.PlayerTeam.First());
                return;
            }
        }
        BattleManager.Instance.TurnSkip.Invoke();
    }
    bool HasUnitInSkillRange(Skill skill)
    {
        int[] arr = { 4, 3, 2, 1 };
        int min = Mathf.Max(0, Position - skill.range - 1);

        for (int i = min; i < 4; i++)
        {
            if (BattleManager.Instance.Units.ContainsKey(arr[i]))
                return true;
        }
        return false;
    }
}