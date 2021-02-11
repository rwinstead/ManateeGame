using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class ServerStageTimer : NetworkBehaviour
{
    public float stageTimeRaw = 60;
    public bool timerIsRunning = false;

    [SerializeField]
    [SyncVar(hook = nameof(RpcUpdateTimer))]
    public string ServerTimerText;

    
    double displayTime;
    public TMP_Text timerText;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopServer()
    {
        Destroy(gameObject);
    }

    public void ServerStartTimer()
    {
        timerIsRunning = true;
    }

    void Update()
    {

        if (!isServer)
        {
            return;
        }

        displayTime = Math.Round(stageTimeRaw, 2);

        if (timerIsRunning)
        {
            if (stageTimeRaw > 0)
            {
                ServerTimerText = "Time: " + displayTime;
                stageTimeRaw -= Time.deltaTime;

            }
            else
            {
                Debug.Log("Timeup!");
                timerIsRunning = false;
                ServerTimerText = "Time: 0.00";


            }
        }
    }

    [ClientRpc]
    private void RpcUpdateTimer(string oldTime, string newTime)
    {
        timerText.text = newTime;
    }


}

