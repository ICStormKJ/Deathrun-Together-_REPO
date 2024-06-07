using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnObjects : Trap
{
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] private float spawnInterval;
    private float t;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isActive.Value) { return; }

        t -= Time.deltaTime;
        if (t <= 0)
        {
            GameObject obj = Instantiate(spawnPrefabs[(int)Random.value * spawnPrefabs.Length], transform.position, Quaternion.identity);
            obj.GetComponent<NetworkObject>().Spawn();
            t = spawnInterval;
        }
    }
}
