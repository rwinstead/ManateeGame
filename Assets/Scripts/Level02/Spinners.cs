using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinners : MonoBehaviour
{

    public float speed = 100;

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
        
        m_EulerAngleVelocity = new Vector3(0, speed, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
    }
}


