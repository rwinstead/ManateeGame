using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;

public class runTimer : NetworkBehaviour
{
    
    public float currentRunStartTime = 0;
    public float currentRunEndTime = 0;
    public float currentRunTimeServer = 0;
    public float bestRunTimeThisSession = 9999999999;
    
    private Canvas InGameCanvas;
    private TMP_Text runTimeText;
    private TMP_Text bestRunTimeText;

    public NetworkGamePlayer thisPlayer;
    private overlord overlord;

    public float currentRunTime = 0;
    public bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        runTimeText = GameObject.Find("CurrentRunTimeUI").GetComponent<TMP_Text>();
        bestRunTimeText = GameObject.Find("BestRunTimeUI").GetComponent<TMP_Text>();
        bestRunTimeText.text = "B: ";

        thisPlayer = GetComponent<LinkToGamePlayer>().thisPlayer;
        overlord = GameObject.FindObjectOfType<overlord>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            if (isRunning)
            {
                currentRunTime += Time.deltaTime;
            }
            runTimeText.text = "C: " + currentRunTime.ToString("n2");
            
        }
        
        
    }

    public void startRun()
    {
        if (hasAuthority)
        {
            if (!isRunning)
            {
                currentRunTime = 0;
                isRunning = true;
                currentRunStartTime = Time.time;
            }
        }
        
        
    }
    
    
    public void endRun(bool successful)
    {
        if (hasAuthority)
        {
            isRunning = false;
            if (successful)
            {

                if (currentRunTime < bestRunTimeThisSession)
                {
                    bestRunTimeThisSession = currentRunTime;
                    bestRunTimeText.text = "B: " + bestRunTimeThisSession.ToString("n2");
                    CMD_reportRunTime(thisPlayer.displayName,bestRunTimeThisSession);
                }
            }
            else
            {
                //Do somehing on failed run (death/timeout/etc)
            }
        }
        
    }

    [Command]
    void CMD_reportRunTime(String playerName,float submissionTime)
    {
        overlord.reportRunTime(playerName, submissionTime);
    }
   
}
