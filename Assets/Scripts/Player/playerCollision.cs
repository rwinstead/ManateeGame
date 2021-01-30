using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class playerCollision : NetworkBehaviour
{

    //[SerializeField] private float knockbackForce = 5f;

    [SerializeField] private Rigidbody rb;

    public BallMove ballMove;

    
    public void handleCollision(Collision collision)
    {
      if(!isServer) { return; }   
        if (collision.gameObject.tag == "Player")
        {

            NetworkIdentity target = collision.gameObject.GetComponentInParent<NetworkIdentity>();

            float otherVelocity = target.GetComponent<BallMove>().velocityBeforeCollision;
            Debug.Log(otherVelocity);

            Rigidbody target_rb = collision.gameObject.GetComponent<Rigidbody>();


            Vector3 dir = (rb.position - collision.gameObject.GetComponent<Rigidbody>().position).normalized;

            float relativeVelocity = collision.relativeVelocity.magnitude;


            relativeVelocity = Mathf.Clamp(relativeVelocity, .1f, 5f);
            float velocityMagnitude = Mathf.Clamp(ballMove.velocityBeforeCollision, .1f, 100f);

            Debug.Log("you were hit by " + otherVelocity);


            RpcAddForce(dir, otherVelocity);


        }

    }

    [ClientRpc]
    public void RpcAddForce(Vector3 dir, float vel)
    {
        rb.AddForce(dir * vel/2f, ForceMode.Impulse);
    }


}
