using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
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
}
