using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Plungers_groupC : NetworkBehaviour
{

    //private float targetY = 244.110f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    //private bool upwards = true;
    private float speed = 0.105f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, (transform.position.y + 5f), transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }




    void FixedUpdate()
    {
        //Debug.Log("Current Position" + transform.position);
        if (transform.position.y >= endPosition.y) { targetPosition = startPosition; }
        else if (transform.position.y <= startPosition.y) { targetPosition = endPosition; }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
    }
}
