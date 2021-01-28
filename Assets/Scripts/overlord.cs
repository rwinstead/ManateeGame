using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using TMPro;

public class Overlord : NetworkBehaviour
{
    /* This class currently only handles the session Time To Beat
     * but is intended to ultimately handle elements that require
     * comparing variables between all players
     * i.e. scoring, leaderboards
     */


    [SyncVar]
    public float TimeToBeat = 9999999;
    [SyncVar]
    public bool RunTimerEnabled = true;

    public NetworkManagerLobby NetworkMan;
    public List<NetworkGamePlayer> PlayerList;

    public TMP_Text TimeToBeatText;

    // Start is called before the first frame update
    void Start()
    {
        //The NetworkManagerLobby object contains the list of NetworkGamePlayer objects
        //Used to reference permenant variables associated with the player
        NetworkMan = GameObject.FindObjectOfType<NetworkManagerLobby>();
        PlayerList = NetworkMan.GamePlayers;

        TimeToBeatText = GameObject.Find("TimeToBeatUI").GetComponent<TMP_Text>();

        this.enabled = true;


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
