using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class ServerStageTimer : NetworkBehaviour
{
    public float stageTimeRaw = 60.00f;
    public bool timerIsRunning = false;

    [SerializeField]
    public double displayTime;

    public TMP_Text timerText;


    private void Start()
    {
        NetworkManagerMG.ServerStopped += UnsubscribeFromEvents;
        NetworkManagerMG.ServerChangedLevel += ResetTimer;
        DontDestroyOnLoad(gameObject);
    }

    public void ServerStartTimer()
    {
        timerIsRunning = true;
    }

    private void ResetTimer()
    {
        timerIsRunning = false;
        stageTimeRaw = 60.00f;
        displayTime = 60.00;
        timerText.text = "Time: 60.00";
        RpcSendTime(displayTime);
    }

    private void UnsubscribeFromEvents() //We have to do manual clean up when server stops, otherwise events won't unsubscribe on server shutdown
    {
        NetworkManagerMG.ServerChangedLevel -= ResetTimer;
        NetworkManagerMG.ServerStopped -= UnsubscribeFromEvents;
        Destroy(gameObject);
    }

    void Update()
    {

        if (!isServer)
        {
            return;
        }

        displayTime = Math.Round(stageTimeRaw, 2);
        RpcSendTime(displayTime);

        if (timerIsRunning)
        {
            if (stageTimeRaw > 0)
            {
                stageTimeRaw -= Time.deltaTime;

            }
            else
            {
                stageTimeRaw = 0.00f;
                Debug.Log("Timeup!");
                timerIsRunning = false;


            }
        }
    }

    [ClientRpc]
    void RpcSendTime(double time)
    {
        timerText.text = String.Format("Time: {0:F2}", time); 
    }

}

