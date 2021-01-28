using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runTrigger_stop : MonoBehaviour
{
    //Pass true if successfully completed run, false if not (i.e. death)
    public bool successful = true;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.GetComponentInParent<NetworkIdentity>().netId.Value);
            other.gameObject.GetComponentInParent<runTimer>().endRun(successful);
        }
    }
}
