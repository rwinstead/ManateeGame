using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointEvent : MonoBehaviour
{

    public Transform startPosition;
    Rigidbody rb;
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
            other.GetComponent<PlayerRespawn>().updateRespawn(this.transform.position);
        }



    }


}