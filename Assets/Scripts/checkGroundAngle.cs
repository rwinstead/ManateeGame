using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkGroundAngle : MonoBehaviour
{

    public LayerMask groundMask;
    public float groundCheckDistance = 20f;
    public float groundAngle;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        getGroundAngle();
    }



    // return a degree 0 to 90 for slope
    private float getGroundAngle()
    {
        groundAngle = -1;

        // define a ray starting at this object's position
        // in this object's local downward direction
        Ray ray = new Ray(transform.position, -transform.up);

        // shoot that ray and get the results
        RaycastHit hit;
        Physics.Raycast(ray.origin, ray.direction, out hit, groundCheckDistance, groundMask);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, groundMask);
        // something was hit
        if (hit.collider != null)
        {

            // get which direction the ground is facing
            Vector3 groundFacingDirection = hit.normal;

            // y axis (when ground is perfectly flat, the normal equals this)
            Vector3 verticalDirection = Vector3.up; // same as Vector3(0,1,0)

            // find the angle between the two directions
            // returns the acute angle (0 to 180)
            groundAngle = Vector3.Angle(verticalDirection, groundFacingDirection);

            // if the angle is greater than 90, make it go back towards 0 instead of up to 180
            if (groundAngle > 90)
            {
                groundAngle = 90 - (groundAngle - 90);
            }
        }
        //Debug.Log(groundAngle);
        return groundAngle;
    }
}