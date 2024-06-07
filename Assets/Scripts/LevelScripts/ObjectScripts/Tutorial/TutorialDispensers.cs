using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDispensers : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    public void Dispenser()
    {
        GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }
}
