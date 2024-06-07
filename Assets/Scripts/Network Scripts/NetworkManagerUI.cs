using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostBTN;
    [SerializeField] private Button clientBTN;
    [SerializeField] private Button serverBTN;

    private void Awake()
    {
        hostBTN.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientBTN.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        serverBTN.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
    }
}
