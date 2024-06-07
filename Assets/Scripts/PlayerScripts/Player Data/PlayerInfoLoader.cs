using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoLoader : MonoBehaviour
{
    //----------Loads back the saved JSON data into the playerdata storage object----------
    private PlayerData retrievedData;
    [SerializeField] private TMP_InputField field;
    void Start()
    {
        retrievedData = PlayerDataStorer.Load(); 
        if (retrievedData == null)
        {
            PlayerDataStorer.Init();
        }
        PlayerData data = FindFirstObjectByType<PlayerData>();

        data.LoadDataBack(retrievedData);
        if (retrievedData != null)
            field.text = retrievedData.name;
    }
}
