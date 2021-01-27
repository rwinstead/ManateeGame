using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class overlord : NetworkBehaviour
{

    public NetworkManagerLobby NetworkMan;
    public List<NetworkGamePlayer> PlayerList;
    // Start is called before the first frame update
    void Start()
    {
        NetworkMan = GameObject.FindObjectOfType<NetworkManagerLobby>();
        PlayerList = NetworkMan.GamePlayers;
        Debug.Log(NetworkMan.thisGuy);

        Debug.Log("Number of Players: " + PlayerList.Count);
        foreach (NetworkGamePlayer player in PlayerList)
        {
            Debug.Log("Wecome Player: "+player.displayName);
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
