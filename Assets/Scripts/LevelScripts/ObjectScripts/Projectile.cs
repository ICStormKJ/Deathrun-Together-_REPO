using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float knockback;

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(this);
    }
}
