
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float turnSpeed = 2.0f;

    public Camera cam;


    void LateUpdate()
    {

        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(new Vector3(target.position.x + 10, target.position.y, target.position.z));


       // transform.Rotate(0f, target.transform.rotation.y, 0f, Space.Self);
        //transform.rotation = target.transform.rotation;
        Debug.Log(target.transform.localRotation.y);

    }
}












//Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
//Plane ground = new Plane(Vector3.up, Vector3.zero);
//float rayLength;

//if (ground.Raycast(cameraRay, out rayLength))
//{
//    //Debug.Log("Rayhit!");
//    Vector3 pointToLook = cameraRay.GetPoint(rayLength);
//    Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

//    Vector3 direction = pointToLook - transform.position;
//    Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
//    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.time);

//    //transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
//}