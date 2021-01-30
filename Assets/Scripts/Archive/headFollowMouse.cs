using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headFollowMouse : MonoBehaviour
{ 
    /*
    public Camera cam;
    public GameObject camera;



    public void Update()
    {
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (ground.Raycast(cameraRay, out rayLength))
        {
            //Debug.Log("Rayhit!");
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            transform.Rotate(0f, 90f, 0f, Space.Self);

            //camera.transform.LookAt(new Vector3(camera.transform.position.x, pointToLook.y, camera.transform.position.z));

        }
    }
    */

}