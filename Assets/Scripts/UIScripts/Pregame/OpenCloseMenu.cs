using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public void Open()
    {
        menu.SetActive(true);
    }

    public void Close()
    {
        menu.SetActive(false);
    }
}
