using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GraphicSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        dropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0;i < filteredResolutions.Count; i++) 
        {
            string resOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }
    public void VSyncToggle(bool vSync)
    {
        if (vSync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    public void ChangeGraphicSetting(int level) //0-5
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void SetResolution(Int32 index)
    {
        Resolution resolution = filteredResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
