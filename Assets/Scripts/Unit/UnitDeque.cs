using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
//플레이어가 가진 유닛의 종류를 담는 클래스 
[System.Serializable]
public class UnitDeque : ISerializableToCSV
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
    /// <summary>
    /// 빠진 index만큼 당겨옵니다.
    /// </summary>
    /// <param name="index"></param>
    public void DeleteUnit(int index)
    {
        list.RemoveAt(index);
    }
    public int GetUnitCount()
     { return list.Count; }
    public string ToCSV()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("UnitDeque");
        for (int i = 0; i < list.Count; i++)
        {
            sb.AppendLine(list[i].ToCSV());
        }
        return sb.ToString();
    }

    public void FromCSV(string data)
    {
        string[] dataArr = data.Split(Environment.NewLine);
        if (dataArr[0] != "UnitDeque")
        {
            Debug.Log($"{data} is not Valid UnitDeque");
            return;
        }

        list.Clear();
        for (int i = 1; i < dataArr.Length; i++)
        {
            if (dataArr[i] == "") continue;
            UnitBase unit = new UnitBase();
            unit.FromCSV(dataArr[i]);
            list.Add(unit);
        }
    }

}

