using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerDataStorer
{
    private static string SAVEFOLDER = Application.dataPath + "/saves/";
    
    //----------Makes directory if doesn't exist----------
    public static void Init() 
    {
        if (!Directory.Exists(SAVEFOLDER)){  Directory.CreateDirectory(SAVEFOLDER); }
    }
    //----------Saves player info to JSON file----------
    public static void Save(PlayerData data)
    {
        if (data == null) return;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SAVEFOLDER + "playerdata.txt", json);
    }
    //----------Loads the info JSON contents back----------
    public static PlayerData Load()
    {
        if (File.Exists(SAVEFOLDER)) 
        {
            string jsonStuff = File.ReadAllText(SAVEFOLDER);
            return JsonUtility.FromJson<PlayerData>(jsonStuff);


        }
        else { Debug.Log("No Save Available! "); return null; }


    }
}
