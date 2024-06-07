using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipManager : MonoBehaviour
{
    //----------Tracking selected and equipped ----------
    private EquipSlot equippedRunnerSlot;
    private EquipSlot equippedTrapSlot;
    private EquipSlot selected;

    //----------Getting info of the borders themselves to track/change location----------
    [SerializeField] private GameObject runnerBorder;
    [SerializeField] private GameObject trapBorder;
    [SerializeField] private GameObject selectBorder;

    [SerializeField] private TMP_Text powerTitle; //desc stuff
    [SerializeField] private TMP_Text description; 

    [SerializeField] private PlayerData dataStorage; //saving and loading equipped power data
    [SerializeField] private List<EquipSlot> slotsToCheck;

    private void Awake()
    {
        DeselectPower();

        //loading back the saved equipped powers
        foreach(EquipSlot s in slotsToCheck)
        {
            if (s.power.Equals(dataStorage.GetRunnerPower()))
            {
                equippedRunnerSlot = s;
                selected = s;
                EquipPower();
            }
            else if (s.power.Equals(dataStorage.GetTrapPower())) 
            {
                equippedTrapSlot = s;
                selected = s;
                EquipPower();
            }
            if (equippedRunnerSlot != null && equippedTrapSlot != null) //if both are already loaded, break out
            {
                selected = null;
                break;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            EquipPower();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectPower();
        }
        
    }

    public void SelectPower() //Function called OnClick to set border, description, etc.
    {
        selectBorder.SetActive(true);
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        selected = btn.GetComponent<EquipSlot>();
        selectBorder.GetComponent<RectTransform>().localPosition = btn.GetComponent<RectTransform>().localPosition;
        description.text = selected.Getdesc();
        powerTitle.text = selected.GetpowerItemName();
    }
    
    void DeselectPower() //reset UI things
    {
        selected = null;
        selectBorder.SetActive(false);
        description.text = "Click to select a power!";
        powerTitle.text = "";
    }

    void EquipPower() //Equip the selected power
    {
        if (selected == null) { return; }

        if (selected.IsTrapPower())
        {
            equippedTrapSlot = selected;
            trapBorder.SetActive(true);
            trapBorder.GetComponent<RectTransform>().localPosition = selectBorder.GetComponent<RectTransform>().localPosition;
        }
        else
        {
            equippedRunnerSlot = selected;
            runnerBorder.SetActive(true);
            runnerBorder.GetComponent<RectTransform>().localPosition = selectBorder.GetComponent<RectTransform>().localPosition;
        }
        selectBorder.SetActive(false);
        
    }

    public void SavePowersOnClose() //Function called when exiting to save equips on exit
    {
        if (equippedRunnerSlot == null || equippedTrapSlot == null) { return; }
        dataStorage.saveRunner(equippedRunnerSlot.power);
        dataStorage.saveTrap(equippedTrapSlot.power);
        DeselectPower();
    }
}

