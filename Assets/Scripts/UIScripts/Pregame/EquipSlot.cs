using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public Power power;
/*    private Image image;
    [SerializeField] private Sprite powerIcon;*/
    [TextArea(3,10)]
    [SerializeField] private string description;
    [SerializeField] private string powerItemName;
    [SerializeField] private bool isTrapmasterPower;

    private void Start()
    {
/*        image = GetComponent<Image>();
        image.sprite = powerIcon;*/
    }

    public string Getdesc(){ return description; }
    public string GetpowerItemName() {  return powerItemName; }
    public bool IsTrapPower(){  return isTrapmasterPower; }
}
