using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonsGameObj;
    private Button[] buttons;

    public void PopUpOpen()
    {
        buttons = buttonsGameObj.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) 
        {
            buttons[i].enabled = false;
        }
    }

    public void PopUpClose()
    {
        if (buttons == null) return;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
    }
}
