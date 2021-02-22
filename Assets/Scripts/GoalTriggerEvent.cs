using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GoalTriggerEvent : NetworkBehaviour
{

    public Transform startPosition;
    public AudioClip goalSound;
    
    private AudioSource audioSource;

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
            if (hasAuthority)
            {
                audioSource.PlayOneShot(goalSound, 0.5f);
            }
        }



    }


}