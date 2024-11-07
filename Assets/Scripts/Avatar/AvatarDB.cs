using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AvatarDB
{
    public static Dictionary<int, AvatarData> avatars;

    public static AvatarData GetAvatarData(int id)
    {
        if(avatars == null)
        {
            
            avatars = CSVReader.ReadAvatarData("CSVs/AvatarDB");
        }
        if(avatars.ContainsKey(id))
            return avatars[id];
        return null;
    }
}
