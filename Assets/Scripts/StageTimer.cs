using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StageTimer : MonoBehaviour
{
    public float stageTimeRaw = 60;
    public bool timerIsRunning = false;

    
    double displayTime;
    public TMP_Text timerText;
    

    private void Start()
    {
        // Starts the timer automatically
        //timerIsRunning = true;
    }

    void Update()
    {

        displayTime = System.Math.Round(stageTimeRaw, 2);

        

        if (timerIsRunning)
        {
            if (stageTimeRaw > 0)
            {
                timerText.text = "Time: " + displayTime;
                stageTimeRaw -= Time.deltaTime;
         
            }
            else
            {
                Debug.Log("Timeup!");
                timerIsRunning = false;
                timerText.text = "Time: 0.00";

                
            }
        }
    }
}
