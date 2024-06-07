using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenManual : MonoBehaviour
{
    [SerializeField]
    private string URL;
    public void OpenLink()
    {
        Application.OpenURL(URL);
    }
}
