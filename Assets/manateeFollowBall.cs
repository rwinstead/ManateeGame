﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manateeFollowBall : MonoBehaviour
{

    public GameObject targ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targ.transform.position, 1f);
    }
}
