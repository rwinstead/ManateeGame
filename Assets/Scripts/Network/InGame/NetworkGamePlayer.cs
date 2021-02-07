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

    public uint myNetId;

    public bool isLeader;

    private NetworkScoreKeeper ScoreKeeper;

    private void Start()
    {
        //if(!hasAuthority) { return; }
        myNetId = gameObject.GetComponent<NetworkIdentity>().netId;

        GiveScoreKeeperNetID();

    }

    public NetworkGamePlayer()
    {
        playerID = System.Guid.NewGuid();    
        
    }
    
    private NetworkManagerMG room;

    public NetworkManagerMG Room
    {
        get
        {
            if(room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton as NetworkManagerMG;
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

    [Server]

    public bool GetIsLeader()
    {
        if (isServer) return true;
        return false;
    }

    [Server]
    public void GiveScoreKeeperNetID()
    {
        ScoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<NetworkScoreKeeper>();
        ScoreKeeper.AddPlayerOnStart(myNetId, displayName);
    }

    [Command]
    public void FinishedRace()
    {
        ScoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<NetworkScoreKeeper>();
        ScoreKeeper.FinishedRace(myNetId);
    }



}
