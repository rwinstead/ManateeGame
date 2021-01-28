using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;
using UnityEngine.UI;

public class NetworkGamePlayer : NetworkBehaviour
{

    [SyncVar]
    public string displayName = "Loading...";

    public Guid playerID;
    public float currentRunStartTime = 0;
    public float currentRunEndTime = 0;
    public float bestRunTimeThisSession = 0;

    public NetworkGamePlayer()
    {
        playerID = System.Guid.NewGuid();    
        
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

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        //base.OnStartClient();
        Room.GamePlayers.Add(this);

    }

    public override void OnStopClient()
    {

        Room.GamePlayers.Remove(this);

    }

   [Server]
   public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }









}
