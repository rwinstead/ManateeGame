using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;

public class Speedometer : NetworkBehaviour
{
    private TMP_Text speedometerText;
    public Rigidbody PlayerRB;


    // Start is called before the first frame update
    void Start()
    {
        speedometerText = GameObject.Find("SpeedometerUI").GetComponent<TMP_Text>();
           
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            speedometerText.text = string.Format("Speed: \n{0:N0}", PlayerRB.velocity.magnitude);
        }
          
    }
}