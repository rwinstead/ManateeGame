using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rotational : NetworkBehaviour
{

    public float speedX = 0;
    public float speedY = 0;
    public float speedZ = 0;

    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        
        m_EulerAngleVelocity = new Vector3(speedX, speedY, speedZ);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
    }
}


