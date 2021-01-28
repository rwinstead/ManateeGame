using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumForce : MonoBehaviour
{

    public float thrust = 1.0f;
    Rigidbody rb;
    public bool direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = true;
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("ChangeThrustDirection", 0.0f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {

       


    }

    void ChangeThrustDirection()
    {
        direction = !direction;
    }

    void FixedUpdate()
    {
        if (direction)
        {
            rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
        }

        else 
        {
            rb.AddForce((transform.forward * -1) * thrust, ForceMode.Impulse);
        }

    }
}
