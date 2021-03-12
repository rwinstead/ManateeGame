using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class StageStartCountdown : NetworkBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private ServerStageTimer serverStageTimer;

    private gameMusic GameMusic;

    public static event Action MovementEnabled;

    private NetworkManagerMG room;
    private NetworkManagerMG Room
    {
        get
        {
            if(room != null) {return room;}
            return room = NetworkManager.singleton as NetworkManagerMG;
        }
    }

    private void Start()
    {
        serverStageTimer = GameObject.FindGameObjectWithTag("RaceHUD").GetComponent<ServerStageTimer>();
        GameMusic = GameObject.Find("GameMusic").GetComponent<gameMusic>();
    }

    public void CountdownOver()
    {
        anim.enabled = false;
    }

    public override void OnStartServer()
    {
        NetworkManagerMG.OnServerReadied += CheckToStartCountdown;
        NetworkManagerMG.ServerStopped += OnStopServer; //We need this otherwise the script can't unsubscribe from the onserverreadied event above.
    }

    public override void OnStopServer()
    {
        NetworkManagerMG.OnServerReadied -= CheckToStartCountdown;
        NetworkManagerMG.ServerStopped -= OnStopServer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        NetworkManagerMG.OnServerReadied -= CheckToStartCountdown;
        NetworkManagerMG.ServerStopped -= OnStopServer;
    }

    [ServerCallback]
    public void EnablePlayerMovement()
    {
        Debug.Log("Player movement enabled");
        RpcEnableMovement();
        serverStageTimer.ServerStartTimer();
        if (GameMusic) { GameMusic.startPlaying(); }
    }

    [Server]

    private void CheckToStartCountdown(NetworkConnection conn)
    {
        foreach(var player in Room.GamePlayers)
        {

            if(player.connectionToClient.isReady == false)
            {
                return;
            }
        }

        anim.enabled = true;

        RpcStartCountdown();

    }

    [ClientRpc]

    private void RpcStartCountdown() //Enables the animator on the client
    {
        anim.enabled = true;
    }

    [ClientRpc]
    private void RpcEnableMovement()
    {
        MovementEnabled?.Invoke();
    }

}
