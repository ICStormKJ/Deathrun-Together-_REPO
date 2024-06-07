using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConstantDispenser : Trap
{
    [SerializeField] private float speed;
    [SerializeField] private float launchFrequency;
    [SerializeField] private GameObject projectilePrefab;
    private float t;
    protected override void Update()
    {
        base.Update();
        if (!isActive.Value) { return; }

        t -= Time.deltaTime;
        if (t <= 0)
        {
            ShootProjectileServerRPC(transform.forward);
            t = launchFrequency;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootProjectileServerRPC(Vector3 direction)
    {
        GameObject obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        obj.GetComponent<NetworkObject>().Spawn(true);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddForce(direction, ForceMode.Impulse);
    }
}
