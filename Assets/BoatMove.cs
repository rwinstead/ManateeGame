﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{

    GameObject boat;
    public CharacterController manatee;
    bool movingBoat;
    bool movingManatee;
    // Start is called before the first frame update
    void Start()
    {
        boat = transform.parent.gameObject;
        movingBoat = false;
        movingManatee = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (movingBoat)
        {
            boat.transform.Translate(Vector3.back * Time.deltaTime * 5f );
        }

        if (movingManatee)
        {
            manatee.Move(Vector3.right * 5f *Time.deltaTime);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("manatee in boat");
        movingManatee = true;
        movingBoat = true;

    }

    void OnTriggerExit(Collider other)
    {
        movingManatee = false;
        movingBoat = false;
    }


}
