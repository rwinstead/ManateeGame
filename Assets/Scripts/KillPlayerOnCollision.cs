using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnCollision : MonoBehaviour
{

    public Transform startPosition;
    public GameObject Player;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered");

        if (other.gameObject.tag == "Player")
        {
            Debug.Log(Player.transform.position);
            Debug.Log(startPosition.position);
         
            rb.position = startPosition.position;
           
        }



    }


}