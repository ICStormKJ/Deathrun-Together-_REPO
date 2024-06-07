using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTraps : MonoBehaviour
{
    /* Class to manage the traps in the tutorial level */
    private bool isActive = false;
    private float timer = 0f;

    //----------Moving platform fields----------
    
    [SerializeField] private TutorialPlatforms[] platforms;

    //----------Dispenser fields----------
    [SerializeField] private float shootingInterval;
    [SerializeField] private TutorialDispensers[] dispensers;
    private float shootTimer;

    private void Update()
    {
        if (!isActive) { return; }
        timer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootingInterval;
            foreach(var dispenser in  dispensers)
            {
                dispenser.Dispenser();
            }
        }
        foreach(var platform in platforms)
        {
            platform.MovePlatforms();
        }
        if (timer <= 0f)
        {
            isActive = false;
            TurnBackOn();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        isActive = true;
        timer = 10.0f;
    }

    private IEnumerator TurnBackOn()
    {
        yield return new WaitForSeconds(5.0f);
        isActive = true;
    }
}
