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
    public string myName = "George";
    
    public NetworkManagerLobby NetworkMan;
    public List<NetworkGamePlayer> PlayerList;
    // Start is called before the first frame update
    void Awake()
    {

        NetworkMan = GameObject.FindObjectOfType<NetworkManagerLobby>();
        PlayerList = NetworkMan.GamePlayers;
        foreach (NetworkGamePlayer player in PlayerList)
        {
            if (player.hasAuthority)
            {
                thisPlayer = player;
                myName = player.displayName;
                //Debug.Log("Hello! My Name is " + thisPlayer.displayName);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
