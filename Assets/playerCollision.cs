using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollision : MonoBehaviour
{

    //[SerializeField] private float knockbackForce = 5f;

    //[SerializeField] private Rigidbody rb;

    private bool hasCollided = false;

   // private float collisionForce;


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);

        if (collision.gameObject.tag == "Player" && !hasCollided)
        {
            hasCollided = true;



            //collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.relativeVelocity, ForceMode.Impulse);
            /*
            Debug.Log(collision.relativeVelocity);

            collisionForce = knockbackForce * collision.relativeVelocity.magnitude;

            Debug.Log(collisionForce);


            
            Debug.Log("hit other player");
            Vector3 dir = collision.contacts[0].point - transform.position;

            dir = -dir.normalized;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(knockbackForce * rb.velocity, ForceMode.Impulse);
            rb.AddForce(dir, ForceMode.Impulse);
            */
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        hasCollided = false;
    }


}
