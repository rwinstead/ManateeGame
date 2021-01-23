using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class rotateManateeInside :NetworkBehaviour
{


    public Transform cam;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform manatee;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) { return; }
        {
            //transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= .01f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(manatee.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                manatee.rotation = Quaternion.Euler(0f, angle, 0f);

            }
        }

        //else
        //{
        //    transform.rotation = transform.rotation;
        //}


        
    }
}
