using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager 
{
    private static List<PlayerData> PlayerDataList = new List<PlayerData>();
    private static string path = Application.persistentDataPath;


    public static void SaveData()
    { 
    }
    public static PlayerData LoadData(int index)
    {
        if(PlayerDataList.Count == 0)
        {
            initialize();
            if (PlayerDataList.Count == 0)
            {
                return null;
            }
        }

        return PlayerDataList[index];
    }

    public static List<PlayerData> GetAllPlayerDatas()
    {
        return PlayerDataList;
    }
    private static void initialize()
    { 

    }
    

}

