using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StageStartCountdown : NetworkBehaviour
{
    public bool CanPlayerMove;

    [SerializeField] private Animator anim;

    private NetworkManagerMG room;
    private NetworkManagerMG Room
    {
        get
        {
            if(room != null) {return room;}
            return room = NetworkManager.singleton as NetworkManagerMG;
        }
    }

    public void CountdownOver()
    {
        anim.enabled = false;
    }

    public override void OnStartServer()
    {
        NetworkManagerMG.OnServerReadied += CheckToStartCountdown;
    }

    public override void OnStopServer()
    {
        NetworkManagerMG.OnServerReadied -= CheckToStartCountdown;
        base.OnStopServer();
    }

    [ServerCallback]
    private void OnDestroy()
    {
        NetworkManagerMG.OnServerReadied -= CheckToStartCountdown;
    }

    [ServerCallback]
    public void EnablePlayerMovement()
    {
        Debug.Log("Player movement enabled");
        RpcEnableMovement();
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

    private void RpcStartCountdown()
    {
        anim.enabled = true;
    }

    [ClientRpc]
    private void RpcEnableMovement()
    {
        CanPlayerMove = true;
    }

}
