using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevel : MonoBehaviour
{
    [SerializeField] private string sceneToOpen;
    
    public void OpenLevel()
    {
        SceneManager.LoadScene(sceneToOpen);
    }
}
