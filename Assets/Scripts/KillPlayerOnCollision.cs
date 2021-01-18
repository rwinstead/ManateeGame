using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnCollision : MonoBehaviour
{

    public Transform startPosition;

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
        //Debug.Log("triggered");

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = startPosition.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }



    }


}