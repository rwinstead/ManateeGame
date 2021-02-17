using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LinkToGamePlayer : NetworkBehaviour
{
    /*  This script is attached to a player controller to 
     *  find and create a link to the NetworkGamePlayer
     *  associated with it.
     */

    public NetworkGamePlayer thisPlayer;

    [SyncVar]
    public string myName = string.Empty;

    [SyncVar]
    public uint GamePlayerNetId;

    private void Start() //Finds which gameplayer this ball belongs to based on the given NetId (provided by playerspawner)
    {
        foreach(var item in NetworkIdentity.spawned)
        {
            if(item.Key == GamePlayerNetId)
            {
                thisPlayer = item.Value.gameObject.GetComponent<NetworkGamePlayer>(); 
            }
        }
    }

    [Server]
    public void SetDisplayName(string name)
    {
        myName = name;
    }

    [Server]
    public void SetOwnersGamePlayerNetID(uint netId)
    {
        GamePlayerNetId = netId;
    }

    public void FinishedRace()
    {
        if(!hasAuthority) { return; }
        thisPlayer.CmdFinishedRace();
    }

}
