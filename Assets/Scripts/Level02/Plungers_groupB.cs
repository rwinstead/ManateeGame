using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Plungers_groupB : NetworkBehaviour
{

    //private float targetY = 244.110f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    private Vector3 platformStart;
    private Vector3 platformDelta;
    //private bool upwards = true;
    private float speed = 0.105f;
    private Rigidbody attachedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, (transform.position.y + 5f), transform.position.z);
        transform.position = endPosition;
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
        platformStart = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
        platformDelta = transform.position - platformStart;
        if (attachedPlayer)
        {
            attachedPlayer.position += platformDelta;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<NetworkIdentity>().hasAuthority)
            {
                attachedPlayer = other.gameObject.GetComponentInParent<Rigidbody>();
                Debug.Log("Attached Player:" + other.gameObject.GetComponentInParent<LinkToGamePlayer>().thisPlayer.displayName);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<NetworkIdentity>().hasAuthority)
            {
                attachedPlayer = null;
                Debug.Log("Detatched Player:" + other.gameObject.GetComponentInParent<LinkToGamePlayer>().thisPlayer.displayName);
            }
        }
    }
}
