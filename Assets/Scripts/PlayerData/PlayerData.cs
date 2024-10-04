using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class PlayerData : ISerializableToCSV
{
    public string playerName;
    public int playCount;
    public  UnitDeque unitDeque;
    public PlayerData()
    {
        unitDeque = new UnitDeque();
    }

    void temp()
    {
        PlayerData pl = new PlayerData();
        JsonUtility.ToJson(pl);
    }
    public string ToCSV()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("PlayerData").Append(",");
        sb.Append(playerName).Append(",");
        sb.Append(playCount).Append("$");
        sb.Append(unitDeque.ToCSV());
        return sb.ToString();
    }

    public void FromCSV(string data)
    {
        string[] dataArr = data.Split('$');
        string[] playerInfo = dataArr[0].Split(",");
        string dequeInfo = dataArr[1];

        if (playerInfo[0] != "PlayerData")
        {
            Debug.Log($"{data} is not Valid PlayerData");
            return;
        }

        playerName = playerInfo[1];
        playCount = int.Parse(playerInfo[2]);
        unitDeque = new UnitDeque();
        unitDeque.FromCSV(dequeInfo);
    }

}


