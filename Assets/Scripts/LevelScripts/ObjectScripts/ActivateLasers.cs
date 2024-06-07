using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActivateLasers : Trap
{
    private Color laserColor;
    private Color thinlaser;
    private Material laserMaterial;

    private void Start()
    {
        laserMaterial = GetComponent<MeshRenderer>().material;
        thinlaser = laserMaterial.color;
        laserColor = new Color(thinlaser.r, thinlaser.g, thinlaser.b, 255);
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (!isActive.Value) { return; }
        timer -= Time.deltaTime;
        if (timer <= duration)
        {
            DisableLasersServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleLaserServerRPC()
    {
        isActive.Value = true;
        timer = duration;
        ToggleLaserClientRPC();
    }

    [ClientRpc(RequireOwnership = false)]
    private void ToggleLaserClientRPC()
    {
        laserMaterial.color = laserColor;
    }

    [ServerRpc(RequireOwnership =false)]
    private void DisableLasersServerRPC()
    {
        isActive.Value = false;
        DisableLasersClientRPC();
    }

    [ClientRpc(RequireOwnership = false)]
    private void DisableLasersClientRPC()
    {
        laserMaterial.color = thinlaser;
    }
}
