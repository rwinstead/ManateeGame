using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 20f;

    //Jump declarations
    public Vector3 jump;
    public float jumpForce = 20.0f;
    public bool isGrounded;
    Rigidbody rb;


    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void Update()
    {

        //transform.parent.position = transform.position - transform.localPosition;

        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.z -= speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);

        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        transform.position = pos;
    }
}




