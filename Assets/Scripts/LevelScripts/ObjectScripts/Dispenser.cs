using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : Trap
{
    [SerializeField] GameObject projectilePrefab;

    //----------Variables that manage interval of shooting and random variables----------
    [Tooltip("How often the projectile spawns and shoots")]
    [SerializeField] private float launchFrequency;
    private float launchTimer;
    [Tooltip("Toggle Y direction of launch angle being randomized | Arc or no arc.")]
    [SerializeField] private bool useRandomY;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isActive.Value) { return; }

        launchTimer -= Time.deltaTime;
        if (launchTimer <= 0)
        {
            //----------Make random floats to input on a timer----------
            Quaternion randDirection = Random.rotation;
            if (!useRandomY)
            {
                randDirection.y = 0;
            }
            ShootProjectile(randDirection, Random.value + 1.0f);

            launchTimer = launchFrequency;
        }
        
    }

    private void ShootProjectile(Quaternion direction, float speed)
    {
        GameObject obj = Instantiate(projectilePrefab, transform.position, direction);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

/*    private void ShootProjectileConstant(Vector3 direction, float speed)
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectilePrefab.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }*/
}
