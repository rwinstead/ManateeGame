﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;

public class runTimer : NetworkBehaviour
{
    public bool isEnabled;    
    
    public float bestRunTimeThisSession = 9999999999;
    
    private TMP_Text runTimeText;
    private TMP_Text bestRunTimeText;

    public NetworkGamePlayer thisPlayer;
    private Overlord overlord;

    public float currentRunTime = 0;
    public bool isRunning = false;

    public Color32 RunningColor = new Color32(255, 255, 255, 255);
    public Color32 SuccessColor = new Color32(0, 255, 0, 255);
    public Color32 FailColor = new Color32(255, 255, 255, 255);

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            overlord = GameObject.FindObjectOfType<Overlord>();
            isEnabled = overlord.RunTimerEnabled;
        }
        catch(Exception e)
        {
            //this is just to make the compiler happy
            e.ToString();

            isEnabled = false;
        }
        //Acquire links to relevant objects
        if (isEnabled) 
        {
            runTimeText = GameObject.Find("CurrentRunTimeUI").GetComponent<TMP_Text>();
            bestRunTimeText = GameObject.Find("BestRunTimeUI").GetComponent<TMP_Text>();
            thisPlayer = GetComponent<LinkToGamePlayer>().thisPlayer;
            bestRunTimeText.text = "B: ";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
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
    }

    public void startRun()
    {
        if (isEnabled)
        {
            if (hasAuthority)
            {
                if (!isRunning)
                {
                    currentRunTime = 0;
                    isRunning = true;
                    runTimeText.color = new Color32(255,255,255,255);
                }
            }
        }
    }
    
    
    public void endRun(bool successful)
    {
        if (isEnabled)
        {
            if (hasAuthority)
            {
                isRunning = false;
                if (successful)
                {
                    
                    if (currentRunTime < bestRunTimeThisSession && currentRunTime != 0)
                    {
                        bestRunTimeThisSession = currentRunTime;
                        bestRunTimeText.text = "B: " + bestRunTimeThisSession.ToString("n2");
                        CMD_reportRunTime(thisPlayer.displayName, bestRunTimeThisSession);
                        runTimeText.color = new Color32(0, 255, 0, 255);
                    }
                }
                else
                {
                    //Do somehing on failed run (death/timeout/etc)
                    runTimeText.color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    [Command]
    void CMD_reportRunTime(String playerName,float submissionTime)
    {
        overlord.reportRunTime(playerName, submissionTime);
    }
   
}
