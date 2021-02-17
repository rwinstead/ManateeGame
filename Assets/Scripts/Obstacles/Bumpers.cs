using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bumpers : NetworkBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    private Vector3 movePosition;
    private Vector3 platformStart;
    private Vector3 platformDelta;
    public float speed = 4f;
    private Rigidbody attachedPlayer;
    private Rigidbody rb;
    private bool exiting = false;
    private float delayTime;
    public initialPosition InitialPosition = initialPosition.Down;
    public float distance;
    public float detatchDelay = 0.3f;
    
    public enum initialPosition
    {
        Up,
        Down
    };
    

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, (transform.position.y + 5f), transform.position.z);
        rb = GetComponent<Rigidbody>();
        if(InitialPosition == initialPosition.Up)
        {
            transform.position = endPosition;
        }

        

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

        movePosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        rb.MovePosition(movePosition);
        
        platformDelta = movePosition - platformStart;

        if (exiting)
        {
            delayTime -= Time.deltaTime;
            if (delayTime <= 0) 
            {
                Debug.Log("Detatched Player:" + attachedPlayer.GetComponentInParent<LinkToGamePlayer>().thisPlayer.displayName);
                
                attachedPlayer = null;
                exiting = false;

            }
        }
        if (attachedPlayer)
        {
            // Debug.Log("platformStart: " + platformStart +" movePosition: " + movePosition + " Delta: " + platformDelta);

            //platformDelta = new Vector3(attachedPlayer.transform.position.x, movePosition.y + distance, attachedPlayer.transform.position.z);
            //attachedPlayer.MovePosition(platformDelta);
            attachedPlayer.MovePosition(attachedPlayer.transform.position + platformDelta * Time.deltaTime);
        }
        
        
    }
   
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(true)
            //if (other.gameObject.GetComponentInParent<NetworkIdentity>().hasAuthority)
            {
                distance = other.transform.position.y - transform.position.y;   
                Debug.Log("Attached Player:" + other.gameObject.GetComponentInParent<LinkToGamePlayer>().thisPlayer.displayName);
                attachedPlayer = other.gameObject.GetComponent<Rigidbody>();
            }
            
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(true)
            //if (other.gameObject.GetComponentInParent<NetworkIdentity>().hasAuthority)
            {
                exiting = true;
                
                delayTime = detatchDelay;
                Debug.Log("Starting detach delay");
            }
        }
    }
    
}
