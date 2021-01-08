﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{

    public Rigidbody rb;
    public Transform cam;
    public float speed = 6f;

    //public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //Gravity

    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    public bool isGrounded;

    public Animator anim;
    Vector3 velocity;

    Vector3 jump;
    public float jumpForce = 60.0f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private object onCollsionExit;

    private void Start()
    {
        
        //anim = this.gameObject.GetComponent<Animator>();
        //Debug.Log(anim);
    }

    // Update is called once per frame
    void Update()
    {
        jump = Vector3.up;
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        Debug.Log(direction);
        //Debug.Log(direction.magnitude);


        //gravity section
        //velocity.y += gravity * Time.deltaTime;
        //rb.MovePosition(transform.position + (velocity * Time.deltaTime));

        //jump section

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            if (direction.x < 0)
            {
                anim.SetTrigger("JumpLeft");
            }

            if (direction.x > 0)
            {
                anim.SetTrigger("JumpRight");
            }

            if (direction.z > 0 && direction.x == 0)
            {
                anim.SetTrigger("FlipForward");
            }

            if (direction.z < 0 && direction.x == 0)
            {
                anim.SetTrigger("FlipForward");
            }

            //velocity.y = Mathf.Sqrt(jumpHeight * -2f);
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            //velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpForce);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Vector3 cforward = cam.transform.forward;
        //Vector3 cright = cam.transform.right;
        //cforward.y = 0f;
        //cright.y = 0f;
        //cforward.Normalize();
        //cright.Normalize();

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //Vector3 movementX = (cam.forward.normalized * horizontal);
        //Vector3 movementZ = (cam.right.normalized * vertical);
        //Vector3 direction = movementX + movementZ;

        //Debug.Log(direction.magnitude);

        if (direction.magnitude >= .01f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //transform.rotation *= Quaternion.Euler(Input.acceleration.y / 6, -Input.acceleration.x / 3, 0);



            rb.AddForce(moveDir.normalized * speed);
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
            Debug.Log("colliding with " + collision.gameObject);
        }

        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("Stopped colliding with " + other.gameObject);
        if (other.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }

  

}