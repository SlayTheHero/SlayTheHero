using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//플레이어가 가진 유닛의 종류를 담는 클래스 
[System.Serializable]
public class UnitDeque
{
    private List<UnitBase> list = new List<UnitBase>();

    public void AddUnit(UnitBase unit)
    {
        list.Add(unit);
    }

    public UnitBase GetUnit(int index)
    {
        if(index  < 0 || index >= list.Count) return null;
        return list[index];
    }
}

