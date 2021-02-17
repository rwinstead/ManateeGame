using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OscellatingPlatform : NetworkBehaviour
{
    public Vector3 positionChange; //Change from initial position
    public float speed = 4f; //How quickly we move
    public initialPosition InitialPosition = initialPosition.Start; //Which end of the cycle the platform begins from
    public float detatchDelay = 0.15f;  //How long the player is affected by the platform after collision exit
    public float platformPauseTimeStart = 0; //How long the platform pauses at the startPosition (in seconds)
    public float platformPauseTimeEnd = 0;  //How long the platform pauses at the endPosition (in seconds)
    public bool debug = false; //Enables Debug Log and Functionality

    //These refer to the entire cycle
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    
    //These are for a specific update frame
    private Vector3 movePosition;
    private Vector3 platformStart;
    private Vector3 platformDelta;
    
    //Script Vars
    private Rigidbody attachedPlayer;
    private Rigidbody rb;
    private bool exiting = false;
    private float exitTime;
    private bool paused = false;
    private float pauseTime;
    
    
    public enum initialPosition
    {
        Start,
        End
    };
    

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3((transform.position.x + positionChange.x), (transform.position.y + positionChange.y), (transform.position.z + positionChange.z));
        
        rb = GetComponent<Rigidbody>();
        
        if(InitialPosition == initialPosition.End)
        {
            transform.position = endPosition;
        }

        

    }

    void FixedUpdate()
    {
        if (debug) //this allows for updating positionChange in the inspector during runtime
        {
            if(endPosition != new Vector3((startPosition.x + positionChange.x), (startPosition.y + positionChange.y), (startPosition.z + positionChange.z)))
            {
                endPosition = new Vector3((startPosition.x + positionChange.x), (startPosition.y + positionChange.y), (startPosition.z + positionChange.z));
                if (InitialPosition == initialPosition.End)
                {
                    transform.position = endPosition;
                }
                else
                {
                    transform.position = startPosition;
                }
            }
            
        }
        if (exiting)
        {
            exitTime -= Time.deltaTime;
            if (exitTime <= 0)
            {
                if (debug) { Debug.Log("Detatched Player:" + attachedPlayer.GetComponentInParent<LinkToGamePlayer>().thisPlayer.displayName); }

                attachedPlayer = null;
                exiting = false;

            }
        }

        if (paused)
        {
            pauseTime -= Time.deltaTime;
            if (debug) { Debug.Log("Platform Paused. Time remaining: "+pauseTime); }
            if (pauseTime <= 0)
            {
                paused = false;
            }
        }
        
        if (!paused)
        {
            if (debug) 
            { 
                Debug.Log("Distance to endPosition: "+Vector3.Distance(transform.position, endPosition));
                Debug.Log("Distance to startPosition: " + Vector3.Distance(transform.position, startPosition));

            }
            //Switches direction if we've reached the end position
            if (Vector3.Distance(transform.position, endPosition) <= 0.05f) 
            { 
                targetPosition = startPosition;
                paused = true;
                pauseTime = platformPauseTimeEnd;
                if (debug) { Debug.Log("endPosition reached. Targeting startPosition."); }
            }
            else if (Vector3.Distance(transform.position, startPosition) <= 0.05f) 
            {
                targetPosition = endPosition;
                paused = true;
                pauseTime = platformPauseTimeStart;
                if (debug) { Debug.Log("startPositon reached. Targeting endPositon."); }
            }

            platformStart = transform.position;
            movePosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(movePosition);
            platformDelta = movePosition - platformStart;

            

            if (attachedPlayer)
            {
                attachedPlayer.MovePosition(attachedPlayer.transform.position + platformDelta * Time.deltaTime);
            }
        }

    }
   
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(true)
            //if (other.gameObject.GetComponentInParent<NetworkIdentity>().hasAuthority)
            {
                if (debug) { Debug.Log("Attached Player:" + other.gameObject.GetComponentInParent<LinkToGamePlayer>().thisPlayer.displayName); }
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
                exitTime = detatchDelay;
                if (debug) { Debug.Log("Starting detach delay"); }
            }
        }
    }
    
}
