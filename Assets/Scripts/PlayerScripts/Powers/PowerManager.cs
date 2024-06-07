using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private static Object cubePowerPrefab;
    [SerializeField] private static float dashDuration;
    private NetworkVariable<float> timer = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private static PlayerManager Pmanager;


    public static void ActivatePower(Power p, PlayerManager player) {
        switch (p)
        {
            case Power.Dash:
                Dash(player);
                break;
            case Power.Speed:
                Speed(player);
                break;
            /*case Power.Hop:
                Hop();
                break;
            case Power.Cube:
                Cube();
                break;
            case Power.Megaphone:
                Megaphone();
                break;
            case Power.Invis:
                InvisibleClientRPC();
                break;
            case Power.Disable:
                Disable();
                break;
            case Power.Technician:
                Technician();
                break;
            case Power.Drain:
                Drain();
                break;*/
            default:
                break;
        }
    }

    static void Dash(PlayerManager player)
    {
        player.dashing = true;
        player.targetpos = player.transform.position + player.GetComponentInChildren<Camera>().transform.forward * 5f;
        player.addPowerCooldown(120.0f);
    }

    static void Speed(PlayerManager player)
    {
        player.GetComponent<PlayerManager>().UpdateSpeeds(1.1f, 1.1f);
    }

    static void Hop(PlayerManager player)
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        player.addPowerCooldown(120.0f);
    }

    static void Cube(PlayerManager player)
    {
        float height = player.GetComponent<PlayerManager>().playerHeight;
        Object cube = Instantiate(cubePowerPrefab, player.transform.position + Vector3.down * (height + 0.2f), player.transform.rotation);
        cube.GetComponent<NetworkObject>().Spawn(true);
        player.addPowerCooldown(240.0f);
    }

    static void Megaphone(PlayerManager player, float radius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(player.transform.position, radius, player.transform.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Player") ;
            hit.transform.GetComponent<PlayerManager>().KnockbackPlayer(1f, player.transform.forward);
        }
        player.addPowerCooldown(210.0f);
    }

    static void Invisible(PlayerManager player)
    {
        Pmanager = player;
        InvisibleClientRPC();
    }

    private static void InvisibleClientRPC()
    {
        Pmanager.GetComponent<MeshCollider>().enabled = false;
    }
    static void Disable(PlayerManager player)
    {
        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClients.Values)
        {
            PlayerManager p = client.OwnedObjects.First().gameObject.GetComponent<PlayerManager>();
            if (!p.Equals(player)){
                p.addPowerCooldown(10.0f);
            }
        }
    }

    static void Technician(PlayerManager player)
    {
        PlayerInteraction t = player.gameObject.GetComponent<PlayerInteraction>();
        if (t == null) { return; }
        t.trapCooldown -= 4.0f;
    }

    static void Drain(PlayerManager player)
    {
        player.UpdateHealthsClientRPC(80.0f);
    }
}
