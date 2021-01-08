using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumForce : MonoBehaviour
{

    public float thrust = 1.0f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);

    }
}
