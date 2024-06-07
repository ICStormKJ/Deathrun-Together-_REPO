using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using System.Data.Common;
using Unity.Services.CloudSave.Models;

public class CloudSave : MonoBehaviour
{

    // Start is called before the first frame update
    public async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async static void SaveData(PlayerData data)
    {
        var dataDict = new Dictionary<string, object>
        {
            {"player data", data}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(dataDict);
    }

    public async static void LoadData()
    {
        Dictionary<string, Item> serverData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{ "player data"});

        if (serverData.ContainsKey("player data"))
            Debug.Log("Key Found with: " + serverData["player data"]);
        else
            Debug.Log("Key not found!");
    }
}
