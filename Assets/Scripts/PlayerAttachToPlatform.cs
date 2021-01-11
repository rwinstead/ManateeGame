using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachToPlatform : MonoBehaviour
{

    public float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.attachedRigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
        }
    }


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        transform.position = transform.position;
    //    }
    //}


}
