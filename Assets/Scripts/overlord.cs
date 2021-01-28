using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using TMPro;

public class overlord : NetworkBehaviour
{
    [SyncVar]
    public float TimeToBeat = 9999999;

    public TMP_Text TimeToBeatText;

    public NetworkManagerLobby NetworkMan;
    public List<NetworkGamePlayer> PlayerList;
    
    
    // Start is called before the first frame update
    void Start()
    {
        NetworkMan = GameObject.FindObjectOfType<NetworkManagerLobby>();
        PlayerList = NetworkMan.GamePlayers;
        Debug.Log(NetworkMan.thisGuy);

        TimeToBeatText = GameObject.Find("TimeToBeatUI").GetComponent<TMP_Text>();

        //Debug.Log("Number of Players: " + PlayerList.Count);
        foreach (NetworkGamePlayer player in PlayerList)
        {
            Debug.Log("Wecome Player: "+player.displayName);
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void reportRunTime(String playerName, float submissionTime)
    {
        if (submissionTime < TimeToBeat)
        {
            cRPC_updateTtB("TtB: " + playerName + " " + submissionTime.ToString("n2"));
            TimeToBeat = submissionTime;
        }
    }

    [ClientRpc]
    public void cRPC_updateTtB(String updateText)
    {
        TimeToBeatText.text = updateText;
    }
}
