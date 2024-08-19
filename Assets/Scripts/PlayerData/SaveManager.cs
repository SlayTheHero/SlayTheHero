using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveManager 
{
    public static int MAX_SAVE_SLOT = 3;
    private static bool isLoaded = false;
    private static bool[] isChanged = new bool[MAX_SAVE_SLOT];
    private static PlayerData[] saveData = new PlayerData[MAX_SAVE_SLOT];
    private static string[] saveStr = new string[MAX_SAVE_SLOT];
    private static string path = Application.persistentDataPath + "/saveFile";


    /// <summary>
    /// index번째 세이브 슬롯에 저장합니다.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    public static void SaveData(PlayerData data, int index)
    {
        if (!isLoaded)
        {
            LoadFileFromClient();
            isLoaded = true;
        }
        if (index < 0 || index >= saveData.Length)
        {
            Debug.Log($"{index} is invalid saveSlot Index");
            return ;
        } 
        saveStr[index] = data.ToCSV();
        isChanged[index] = true;
    }
    /// <summary>
    /// index번째 데이터가 존재하면 가져오고 없으면 null을 반환합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static PlayerData LoadData(int index)
    {
        if (index < 0 || index >= saveData.Length)
        {
            Debug.Log($"{index} is invalid saveSlot Index");
            return null;
        }
        if (!isLoaded)
        {
            LoadFileFromClient();
            isLoaded = true;
        }
        if (saveStr[index] == null) 
        { 
            return null;
        }


        if (isChanged[index])
        {
            PlayerData playerData = new PlayerData();
            playerData.FromCSV(saveStr[index]);
            saveData[index] = playerData;
            isChanged[index] = false;
        }


        return saveData[index];
    }

    /// <summary>
    /// index번째 데이터를 제거합니다.
    /// </summary>
    /// <param name="index"></param>
    public static void DeleteData(int index)
    {
        if (!isLoaded)
        {
            LoadFileFromClient();
            isLoaded = true;
        }
        if (index < 0 || index >= saveData.Length)
        {
            Debug.Log($"{index} is invalid saveSlot Index");
            return;
        }
        saveStr[index] = null;
        isChanged[index] = true;
    }

     
    /// <summary>
    /// 게임종료나 로딩시 데이터를 한번에 클라이언트에 저장합니다.
    /// </summary>
    public static void SaveFileToClient()
    {
        if (!isLoaded)
        {
            LoadFileFromClient();
            isLoaded = true;
        }
        for (int i = 0; i < MAX_SAVE_SLOT; i++)
        {
            string filePath = path + $"{i + 1}.txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (saveStr[i] == null)
            {
                continue;
            }    
            StreamWriter sw = File.CreateText(filePath);
            sw.WriteLine(saveStr[i]);
            Debug.Log($"save Completed at path :{filePath}");
            sw.Close();
        }
    }

    /// <summary>
    /// 클라이언트의 데이터를 가져옵니다.
    /// </summary>
    private static void LoadFileFromClient()
    {
        Array.Fill(isChanged, false);
        for (int i = 0; i < MAX_SAVE_SLOT; i++)
        {
            string filePath = path + $"{i + 1}.txt";
            if (File.Exists(filePath))
            {
                saveStr[i] = File.ReadAllText(filePath);
                PlayerData playerData = new PlayerData();
                playerData.FromCSV(saveStr[i]);
                saveData[i] = playerData;
            }
            continue;
        }
    }


    

}

