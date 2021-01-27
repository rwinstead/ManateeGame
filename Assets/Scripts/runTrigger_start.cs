using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runTrigger_start : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.GetComponentInParent<NetworkIdentity>().netId.Value);
            
            other.gameObject.GetComponentInParent<runTimer>().startRun();
        }
    }
}
