using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class playerCollision : NetworkBehaviour
{

    [SerializeField] private float knockbackForce = 2f;

    [SerializeField] private Rigidbody rb;

    public BallMove ballMove;

    
    public void HandleCollision(Collider collision)
    {
      if(!isServer) { return; }   
        if (collision.gameObject.CompareTag("Player"))
        {

            NetworkIdentity target = collision.gameObject.GetComponentInParent<NetworkIdentity>();

            Vector3 otherVelocity = target.GetComponent<BallMove>().velocityBeforeCollision;

            Vector3 myVelocity = ballMove.velocityBeforeCollision;

            Vector3 resultantDir = CompareVelocities(myVelocity, otherVelocity);

            Vector3 dir = (rb.position - collision.gameObject.GetComponentInParent<Rigidbody>().position).normalized; // This gets the angle perpendicular from the collision point. Used for slight knockback

            RpcAddForce(resultantDir/3f); // Force vector in direction of other ball's velocity (after comparing velocities)
            RpcAddForce(dir*knockbackForce); // Force vector perpendicular to collision point.

        }

    }

    // This function compares the velocites bewteen the two colliding bodies. It applies the difference
    // in velocities in each direction if one is greater. Otherwise, no force is applied (since the faster 
    // body is the one applying a force in that direction).

    private Vector3 CompareVelocities(Vector3 myVelocity, Vector3 otherVelocity)
    {
        Vector3 resultant = Vector3.zero;

        if (Mathf.Abs(otherVelocity.x) > Mathf.Abs(myVelocity.x))
        {

            if ((otherVelocity.x < 0 && myVelocity.x > 0) || (otherVelocity.x > 0 && myVelocity.x < 0))
            {
                resultant.x = otherVelocity.x * .8f;
            }

            else
            {
                resultant.x = Mathf.Abs(otherVelocity.x) - Mathf.Abs(myVelocity.x);
                if (otherVelocity.x < 0) resultant.x *= -1f;
            }
        }

        if (Mathf.Abs(otherVelocity.y) > Mathf.Abs(myVelocity.y))
        {

            if ((otherVelocity.y < 0 && myVelocity.y > 0) || (otherVelocity.y > 0 && myVelocity.y < 0))
            {
                resultant.y = otherVelocity.y * .8f;
            }

            else
            {
                resultant.y = Mathf.Abs(otherVelocity.y) - Mathf.Abs(myVelocity.y);
                if (otherVelocity.y < 0) resultant.y *= -1f;
            }
        }

        if (Mathf.Abs(otherVelocity.z) > Mathf.Abs(myVelocity.z))
        {
            if ((otherVelocity.z < 0 && myVelocity.z > 0) || (otherVelocity.z > 0 && myVelocity.z < 0))
            {
                resultant.z = otherVelocity.z * .8f;
            }

            else
            {
                resultant.z = Mathf.Abs(otherVelocity.z) - Mathf.Abs(myVelocity.z);
                if (otherVelocity.z < 0) resultant.z *= -1f;
            }
        }

        return resultant;
    }



    [ClientRpc]
    public void RpcAddForce(Vector3 dir)
    {
        rb.AddForce(dir, ForceMode.Impulse);
    }


}
