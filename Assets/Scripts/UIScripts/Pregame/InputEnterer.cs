using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputEnterer : MonoBehaviour
{
    //----------Class to manage confirm buttons for display name setting----------
    [SerializeField] private TMP_InputField field;
    private PlayerData playerData;
    [SerializeField] private LobbySetUp lobbyManager;

    private void Start()
    {
        playerData = FindFirstObjectByType<PlayerData>();
    }
    public void SubmitDisplayName()
    {
        playerData.UpdateDisplayName(field.text);
    }
    public void SubmitLobbyCode()
    {
        lobbyManager.AttemptJoinLobbyCode(field.text);
    }
}
