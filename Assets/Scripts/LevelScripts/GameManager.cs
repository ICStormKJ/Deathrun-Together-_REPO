using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using System;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    //-----Intermission fields-----
    [SerializeField] private GameObject invisibleWall;
    [SerializeField] private float intermissionTime;

    //-----barrier fields-----
    [SerializeField] private GameObject barrier;
    [SerializeField] private float timeToSpawnBarrier;

    //-----fields for spawning-----
    [SerializeField] private Transform runnerSpawnPoint;
    [SerializeField] private Transform trapMasterSpawnPoint;

    [SerializeField] private GameObject endZoneBarrier; //the barrier that spawns on each instance individually when they goal

    [SerializeField] private GameObject finishedGameUI; //the result menu
    [SerializeField] private GameObject finishedGameText; //the text that says "Finished!" when the game ends, like an announcer.

    private float timerForThings; //timer used to end intermission and spawn the barrier
    private static PlayerManager playerman; //the playermanager of this instance's player

    [SerializeField] private Camera mainMenuCamera;

    //-----connected clients and playercount fields
    public NetworkVariable<int> playersEliminated = new NetworkVariable<int>(0);
    private NetworkVariable<int> playersGoaled = new NetworkVariable<int>(0);
    private IReadOnlyDictionary<ulong, NetworkClient> clientsConnected;
    private NetworkVariable<int> playersPresent = new NetworkVariable<int>(0);

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) enabled = false;
        timerForThings = 0.0f;

        mainMenuCamera.gameObject.SetActive(false);
        invisibleWall.SetActive(true);
        barrier.SetActive(false);
        endZoneBarrier.SetActive(false);
        finishedGameUI.SetActive(false);

        if (IsHost) { clientsConnected = NetworkManager.Singleton.ConnectedClients; }
        playersPresent.Value = clientsConnected.Count;
        chooseRandomPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        timerForThings += Time.deltaTime;
        if (invisibleWall.active && timerForThings >= intermissionTime)
        {
            invisibleWall.SetActive(false);
        }
        if (timerForThings >= timeToSpawnBarrier)
        {
            if (barrier.active)
            {
                //*****MAKE BARRIER MOVE ALONG A CERTAIN PATH*****
            }
            else
                barrier.SetActive(true);
        }

        if (playersEliminated.Value + playersGoaled.Value + 1 == playersPresent.Value)
        {
            EndGame();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Goal"))
        {
            ReachEndZone();
        }
    }

    private void chooseRandomPlayer()
    {
        ulong randomIndex = (ulong) UnityEngine.Random.Range(0,playersPresent.Value);
        GameObject playerObj = clientsConnected[randomIndex].OwnedObjects.First().gameObject;
        playerObj.GetComponent<PlayerManager>().MakeTrapmaster();
    }
    
    //[ClientRpc]
    /*    private void SpawnAllPlayersClientRPC() //calls joinrelay on all clients for them to join and become clients
        {
            JoinRelay(relayCode);
        }*/

    //----------Function called from the trigger in the end zone to do stuff to the player----------
    public void ReachEndZone() 
    {
        endZoneBarrier.SetActive(true);
        playerman.addPowerCooldown(999f);
        playersGoaled.Value++;
    }
    //----------Method to display the UI, disconnect everyone and then open the previous level-----------
    private void EndGame()
    {
        DisplayGameFinishedUIClientRPC();
        StartCoroutine(DelayShutDown());
        
    }

    private IEnumerator DelayShutDown()
    {
        yield return new WaitForSeconds(5.0f);
        NetworkManager.Singleton.Shutdown();
    }

    [ClientRpc]
    private void DisplayGameFinishedUIClientRPC()
    {
        finishedGameUI.GetComponentInParent<Canvas>().gameObject.SetActive(true);
        finishedGameText.SetActive(true);
        DisplayResultScreen();
    }

    private IEnumerator DisplayResultScreen()
    {
        yield return new WaitForSeconds(5.0f);
        finishedGameText.SetActive(false);
        finishedGameUI.SetActive(true);

    }
    public static void SetPlayerManager(PlayerManager manager)
    {
        playerman = manager;
    }
}
