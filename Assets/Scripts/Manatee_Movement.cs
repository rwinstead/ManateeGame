using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manatee_Movement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;

    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //Gravity
    Vector3 velocity;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    bool isGrounded;

    public Animator anim;


    private void Start()
    {
        //anim = this.gameObject.GetComponent<Animator>();
        //Debug.Log(anim);
    }

    // Update is called once per frame
    void Update()
    {

    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        Debug.Log(direction.x);

    if (direction.magnitude >= .01f)
    {

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }


        //gravity section
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //jump section

        if(Input.GetButtonDown("Jump") && isGrounded)
        {

            if (direction.x < 0)
            {
                anim.SetTrigger("JumpLeft");
            }

            if (direction.x > 0)
            {
                anim.SetTrigger("JumpRight");
            }

            if(direction.z > 0 && direction.x == 0)
            {
                anim.SetTrigger("FlipForward");
            }

            if (direction.z < 0 && direction.x == 0)
            {
                anim.SetTrigger("FlipForward");
            }

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }



        


    }
}
