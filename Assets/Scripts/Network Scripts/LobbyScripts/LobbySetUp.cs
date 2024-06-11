using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
using static LobbyManager;

public class LobbySetUp : MonoBehaviour
{
    [SerializeField] private int capacity;

    //-----fields to manage the heartbeats and lobby
    private Lobby hostLobby;
    private Lobby currentLobby;
    private float heartbeatTimer;
    [SerializeField] private float heartbeatTimerMax;

    //-----Setting UI elements-----
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text roomCode;
    //[SerializeField] private RoomSlot[] slots;
    [SerializeField] private GameObject lobbyWarning;

    [SerializeField] private GameObject mainCanvas;

    //-----Fields used when spawning players and level
    [SerializeField] Transform runnerSpawn;
    [SerializeField] Transform trapSpawn;
    [SerializeField] GameObject levelToActivate;

    [SerializeField] private OpenCloseMenu menu;

    public static LobbySetUp Instance;
    //public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    private PlayerData data;


    private void Awake()
    {
        Instance = this;
        data = FindFirstObjectByType<PlayerData>();
    }
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in yay: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        heartbeatTimer = heartbeatTimerMax;
        levelToActivate.SetActive(false);
        lobbyWarning.SetActive(false);

        //LobbyManager.Instance.OnJoinedLobby += UpdateLobbyEvent;
    }

    private void Update()
    {
        HeartbeatManager();
        if (currentLobby == null || currentLobby.Players.Count < 2) { startGameButton.interactable = false;}
        else { startGameButton.interactable=true; }
    }

    //----------Keeps Lobby alive lol----------
    private async void HeartbeatManager()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0f) 
            {
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public async void CreateLobby(string name)
    {
        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions()
            { //LOBBY OPTIONS
                IsPrivate = true,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "Start", new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            }; //END OF LOBBY OPTIONS

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(name, capacity, options);
            var callbacks = new LobbyEventCallbacks();
            //callbacks.PlayerDataAdded += UpdateLobbyEvent;
            hostLobby = lobby;
            currentLobby = hostLobby;

            Debug.Log("Created Lobby: " + lobby.LobbyCode);
            //PlayerList(lobby);
        } catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void AttemptJoinLobbyCode(string code)
    {
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions()
            {
                Player = GetPlayer(),
            };
            currentLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code,options);
            if (currentLobby != null)
                menu.Open();
            else
                ShowWarning();
            Debug.Log(menu.isActiveAndEnabled);
        }
        catch (LobbyServiceException ex) { ShowWarning(); Debug.Log(ex); }    
    }




    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"Player Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, data.displayName) }
                    }
        };
    }
    public async void StartGame()
    {
        try
        {
            string relayCode = CreateRelay().Result;
            //----------retrieves the relayCode required, checks if we have it, and starts the game.
            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                    {"Start", new DataObject (DataObject.VisibilityOptions.Member,relayCode) }
                }

            });
            currentLobby = lobby;
            levelToActivate.SetActive(true); //activates the level with the actual stuff
            if (!NetworkManager.Singleton.IsHost)
            {
                JoinRelay(currentLobby.Data["Start"].Value);
                SendPosToServerRPC(runnerSpawn.position.x, runnerSpawn.position.y, runnerSpawn.position.z);
            }
            else
            {
                SendPosToServerRPC(trapSpawn.position.x, trapSpawn.position.y, trapSpawn.position.z);
            }

            if (currentLobby.Data["Start"].Value != "0")
            {
                mainCanvas.SetActive(false);
                FindFirstObjectByType<GameManager>().gameObject.SetActive(true);
            }

            currentLobby = null;
        }
        catch (LobbyServiceException ex) { Debug.Log(ex); }
    }

    [ServerRpc]
    private void SendPosToServerRPC(float x, float y, float z) //serverRPC to manage the player's position when spawning
    {
        transform.position = new Vector3(x, y, z);
    }
    //----------creates a relay allocation and connects to network manager----------
    private async Task<string> CreateRelay()
    {
        try
        {
            Allocation alloc =  await RelayService.Instance.CreateAllocationAsync(9);

            //creates and sets server relay data so that relay can connect to network transform and stuff
            RelayServerData data = new RelayServerData(alloc, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
            NetworkManager.Singleton.StartHost();
            return await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            

        }catch(RelayServiceException ex) { Debug.Log(ex); return null; }
    }
    //----------takes in the code used to attempt to join the relay needed to connect in a lobby----------
    private async void JoinRelay(string relayCode)
    {
        try
        {
            JoinAllocation joinalloc = await RelayService.Instance.JoinAllocationAsync(relayCode);

            RelayServerData data = new RelayServerData(joinalloc, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
            NetworkManager.Singleton.StartClient(); 

        }
        catch (RelayServiceException ex) { Debug.Log(ex);}
    }

    private IEnumerator ShowWarning()
    {
        lobbyWarning.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        lobbyWarning.SetActive(false);
    }



}
