using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTriggerEvent : MonoBehaviour
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
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(Player.transform.position);
            Debug.Log(startPosition.position);

            rb.position = startPosition.position;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }



    }


}