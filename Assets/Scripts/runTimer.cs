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
    
    private GameObject CanvasGameObject;
    private Canvas InGameCanvas;
    private TMP_Text runTimeText;
    private TMP_Text bestRunTimeText;


    public float currentRunTime = 0;
    public bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
       //InGameCanvas = 
        runTimeText = GameObject.Find("CurrentRunTimeUI").GetComponent<TMP_Text>();
        bestRunTimeText = GameObject.Find("BestRunTimeUI").GetComponent<TMP_Text>();
        runTimeText.text = "C: 9.99";
        bestRunTimeText.text = "B: ";
        Debug.Log("Found text field: "+runTimeText.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            currentRunTime += Time.deltaTime;
        }

       runTimeText.text = "C: "+currentRunTime.ToString("n2");
        
    }

    public void startRun()
    {
        if (hasAuthority)
        {
            currentRunTime = 0;
            isRunning = true;
            CMD_startRun();
        }
        
        
    }
    
    
    public void endRun(bool successful)
    {
        if (hasAuthority)
        {
            isRunning = false;
            CMD_endRun(successful);
        }
        
    }

    [Command]
    void CMD_startRun()
    {
        currentRunStartTime = Time.time;
        Debug.Log("Run Started");
    }
    [Command]
    void CMD_endRun(bool successful)
    {
        if (successful)
        {

            currentRunEndTime = Time.time;
            currentRunTimeServer = currentRunEndTime - currentRunStartTime;
            Debug.Log("Run Finished. Time: " + currentRunTimeServer);
            if (currentRunTimeServer < bestRunTimeThisSession)
            {
                bestRunTimeThisSession = currentRunTimeServer;
                bestRunTimeText.text = "B: " + bestRunTimeThisSession.ToString("n2");
            }
        }
        else
        {
            //Do somehing on failed run (death/timeout/etc)
        }
    }
    
}
