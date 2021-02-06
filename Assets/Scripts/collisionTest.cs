using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        Debug.Log(collision.gameObject + "  " + "force: " + collisionForce);
    }
}
