using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class runTimer : NetworkBehaviour
{

    private float runTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        runTime += Time.delatTime();
    }

    

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.GetComponentInParent<NetworkIdentity>().netId.Value);
        }
    }
    
}
