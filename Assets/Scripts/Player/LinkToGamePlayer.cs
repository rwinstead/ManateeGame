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


    [SyncVar]
    public NetworkGamePlayer thisPlayer;

    [SyncVar]
    public string myName = string.Empty;

    private NetworkManagerMG room;

    public NetworkManagerMG Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton as NetworkManagerMG;
        }
    }

    private void Start()
    {
        if (isServer)
        {
            foreach(NetworkGamePlayer Player in Room.GamePlayers)
            {

                if(gameObject.GetComponent<NetworkIdentity>().connectionToClient == Player.connectionToClient)
                {
                    thisPlayer = Player;
                }

                Debug.Log(gameObject.GetComponent<NetworkIdentity>().connectionToClient);
                Debug.Log(Player.connectionToClient);
            }
        }
    }


    [Server]
    public void SetDisplayName(string name)
    {
        myName = name;
    }

    [Server]
    public void SetClientGamePlayer(NetworkGamePlayer player)
    {
        thisPlayer = player;
    }

    public void FinishedRace()
    {
        if(!hasAuthority) { return; }
        thisPlayer.CmdFinishedRace();
    }

}
