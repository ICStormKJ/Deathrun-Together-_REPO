using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private KeyCode optionsKey = KeyCode.Escape;
    private OpenCloseMenu popUp;
    private bool menuOpen;

    void Start()
    {
        popUp = GetComponent<OpenCloseMenu>();
        menuOpen = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(optionsKey))
        {
            if (!menuOpen)
            {
                popUp.Open();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                popUp.Close();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            menuOpen = !menuOpen;
        }
            
    }
}
