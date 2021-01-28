using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class NetworkRoomPlayer : NetworkBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private TMP_Dropdown mapSelectorDropdown = null;


    //For the two syncvars below: the hook parameter means that when the client gets an update 
    //on the variable beneath it (either the string DisplayName or bool IsReady),
    // invoke the method that is the hook. So whenever the client gets a change
    // on IsReady or DisplayName, it will automatically invoke the method.

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
            mapSelectorDropdown.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if(room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        //base.OnStartAuthority();
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        lobbyUI.SetActive(true);

    }

    public override void OnStartClient()
    {
        //base.OnStartClient();
        Room.RoomPlayers.Add(this);

        UpdateDisplay();

    }

    public override void OnStopClient()
    {

        Room.RoomPlayers.Remove(this);

        UpdateDisplay();

    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for player...";
            playerReadyTexts[i].text = string.Empty;
        }        

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not ready</color>";
        }


    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();


    }

    [Command]

    public void setMap()
    {
        Room.selectMap(mapSelectorDropdown.value);
    }







}
