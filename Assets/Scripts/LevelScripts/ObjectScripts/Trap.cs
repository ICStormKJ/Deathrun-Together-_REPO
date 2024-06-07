using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Trap : NetworkBehaviour 
{
    /* Base class for all traps, with the general isActive management and duration of trap, as well as timer. All traps inherit this + their own properties */
    protected NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] protected static float duration;
    protected float timer = 0f;

    protected virtual void Update()
    {
        if (!isActive.Value) { return; }
        timer -= Time.deltaTime;
        if (timer <= duration)
        {
            isActive.Value = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActivateTrapServerRPC()
    {
        isActive.Value = true;
        timer = duration;
    }

}
