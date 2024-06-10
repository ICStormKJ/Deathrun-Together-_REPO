using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlot : MonoBehaviour
{

    public string nameOnSlot = "";
    public Image icon;
    private Transform slotLocation;
    private bool occupied;
    // Start is called before the first frame update

    public RoomSlot(string name)
    {
        nameOnSlot = name;
        occupied = true;
        GetComponent<TMP_Text>().text = name;
    }
    private void Start()
    {
        slotLocation = transform;
    }

    public RoomSlot(string name, Sprite img)
    {
        nameOnSlot = name;
        occupied = true;
        GetComponent<TMP_Text>().text = name;
        icon.sprite = img;
        icon.transform.position = slotLocation.position + (Vector3.right * 80);
    }

    public void RemovePlayer()
    {
        nameOnSlot = "";
        occupied = false;
        GetComponent<TMP_Text>().text = "";
    }

    public void MovePlayer(Vector3 newpos)
    {
        slotLocation.position = newpos;
    }

    public bool isOccupied() { return occupied; }
}
