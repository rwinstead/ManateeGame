﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTriggerEvent : MonoBehaviour
{

    public Transform startPosition;
    //public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<PlayerRespawn>().Respawn();
        }



    }


}