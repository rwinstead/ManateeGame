using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class BallMove : NetworkBehaviour
{

    public Rigidbody rb;
    public Transform cam;
    
    public float speed = 6f;
    public float groundedFriction;

    public checkGroundAngle checkGroundAngle;
    public float slopeAcceleration;


    //public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;

    //Gravity

    public Transform groundCheck;
    public LayerMask groundMask;

    public float groundDistance = 0.4f;

    public bool isGrounded;

    public Animator anim;
    Vector3 velocity;

    public float gravity = -20f;

    Vector3 jump;
    public float jumpForce = 60.0f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    
    public float velocityBeforeCollision;

    
    private void Start()
    {
        Physics.gravity = new Vector3(0, gravity, 0);
        rb.maxAngularVelocity = Mathf.Infinity;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) { return; } // this ensures that player only controls their character on server
        
           //Debug.Log(rb.angularVelocity.magnitude);

            jump = Vector3.up;
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;

            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            //Debug.Log(direction);


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


                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            }

            if (Input.GetButtonDown("ResetCharacter"))
                {
                    Debug.Log("Reset!");
                    GetComponentInParent<PlayerRespawn>().Respawn();
                    GetComponentInParent<runTimer>().endRun(false);
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

        velocityBeforeCollision = rb.velocity.magnitude;

        if (!hasAuthority) { return; } // this ensures that player only controls their character on server

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= .01f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * speed);
        }

        if (rb.velocity.magnitude > .1 && isGrounded)
        {
            rb.velocity *= groundedFriction;
        }

        if (checkGroundAngle.groundAngle > .5)
        {
            rb.AddForce(Vector3.down * checkGroundAngle.groundAngle * slopeAcceleration, ForceMode.Acceleration);
        }

    }

    public void collisionWithOtherPlayer(Vector3 dir, float force)
    {
        //if (!hasAuthority) { return; }
        {
            Debug.Log("You were hit. Force " + force + "Dir " + dir);
            rb.AddForce(dir * force * 3f, ForceMode.Impulse);
        }
    }


    // THIS SECTION CHECKS FOR THE GROUND VIA COLLISION *****************************
    /*
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
    */

}