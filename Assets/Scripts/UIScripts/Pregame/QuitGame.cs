using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        PlayerDataStorer.Save(FindFirstObjectByType<PlayerData>());

        Application.Quit();
        Debug.Log("Quitting game");
    }
}
