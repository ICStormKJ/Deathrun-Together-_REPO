using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPower : MonoBehaviour
{
    public static void DashTutorial(TutorialPlayer player)
    {
        player.dashing = true;
        player.targetpos = player.transform.position + (player.GetComponentInChildren<Camera>().transform.forward * 5.0f);
        player.addPowerCooldown(5.0f);
    }
}
