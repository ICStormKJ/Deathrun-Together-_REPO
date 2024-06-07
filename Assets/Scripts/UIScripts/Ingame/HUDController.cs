using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_Text interactionText;
    //----------Enable and disable the text from raycast----------
    public void EnableText(string text)
    {
        interactionText.text = text;
        interactionText.gameObject.SetActive(true);
    }

    public void DisableText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
