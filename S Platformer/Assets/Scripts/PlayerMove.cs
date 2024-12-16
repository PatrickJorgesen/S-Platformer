using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveCap = 30;
    [SerializeField] int airJumpAmount = 2;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float bufferTime = 0.2f;
    [SerializeField] float groundDeceleration = 5;
    [SerializeField] float startingSlideSpeed = 1;
    [SerializeField] float slideDropOff = 0.01f;
    [SerializeField] float wallJumpDistance = 1f;
    [SerializeField] GameObject downRay;
    [SerializeField] GameObject forwardRay;
    [SerializeField] ContactFilter2D groundFilter;

    int jumpCount = 0;
    bool slidbuffer = false;
    bool sliding = false;
    bool canSlide = true;
    bool isGrounded;
    bool isCrouching = false;
    bool jumpBuffer = false;
    Vector2 moveDir;
    Vector2 mainVector;
    RaycastHit2D groundRay;
    RaycastHit2D wallRay;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = new Vector2(mainVector.x, rb.velocity.y);
        isGrounded = rb.IsTouching(groundFilter);
        //Walking and sliding
        if (sliding == false && isGrounded == true) //Player is walking
        {
            if (mainVector.x < moveCap && mainVector.x > -moveCap)
            {
                mainVector.x += moveSpeed * moveDir.x * Time.deltaTime;
            }
            if (mainVector.x > 0)
                mainVector.x -= groundDeceleration * Time.deltaTime;
            else if (mainVector.x < 0)
                mainVector.x += groundDeceleration * Time.deltaTime;
        }
        else if (isGrounded == true) //player is sliding
        {
            if (mainVector.x > 0)
                mainVector.x -= (groundDeceleration + slideDropOff) * Time.deltaTime;
            else if (mainVector.x < 0)
                mainVector.x += (groundDeceleration + slideDropOff) * Time.deltaTime;
        }

        //misc
        if (isGrounded) 
        {
            jumpCount = airJumpAmount;
            if (jumpBuffer == true) // Activates the buffer jump
            {
                jumpBuffer = false;
                Jump();
            }
            if (slidbuffer == true)
            {
                slidbuffer = false;
                Slide();
            }
        }
        // The Debug sectio
        //Debug.Log(mainVector.x);
    }
    private void FixedUpdate()
    {
        groundRay = Physics2D.Raycast(downRay.transform.position, -Vector2.up);

        Vector2 direction = new Vector2(transform.localScale.x, 0); // Determine direction
        wallRay = Physics2D.Raycast(forwardRay.transform.position, direction);

        // Debug visualization for groundRay
        if (groundRay.collider != null)
        {
            Debug.DrawRay(downRay.transform.position, -Vector2.up * groundRay.distance, Color.magenta);
        }

        // Debug visualization for wallRay
        if (wallRay.collider != null)
        {
            Debug.DrawRay(forwardRay.transform.position, direction * wallJumpDistance, Color.magenta);
        }
    }
    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        if (moveDir.x == 1)
        {
            rb.transform.localScale = new Vector3(1, rb.transform.localScale.y, 1);
        }
        else if (moveDir.x == -1)
        {
            rb.transform.localScale = new Vector3(-1, rb.transform.localScale.y, 1);
        }
    }
    void OnCrouch(InputValue Value)
    {
        isCrouching = Value.isPressed;
        Crouch();
    }
    void OnJump()
    {
        canSlide = true;
        if (isGrounded)
        {
            Jump();
        }
        else if (wallRay.distance < wallJumpDistance && groundRay.distance > bufferTime)
        {
            mainVector.x = -mainVector.x;
            Jump();
        }
        else if (groundRay.distance < bufferTime) //Buffers the players jump
        {
            jumpBuffer = true;
        }
        else if (jumpCount > 0)
        {
            Jump();
            jumpCount--;
        }
    }
    void Crouch()
    {
        if (isCrouching == true)
        {
            if (isGrounded == true && moveDir.x != 0 && canSlide == true) // activates sliding
            {
                Slide();
            }
            else if (groundRay.distance < bufferTime && moveDir.x != 0 && canSlide == true) //Buffers the players slide
            {
                slidbuffer = true;
            }
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, 0.75f, rb.transform.localScale.z);
            isCrouching = true;
        }
        else if (isCrouching == false)
        {
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, 1f, rb.transform.localScale.z);
            if (isCrouching == true)
                isCrouching = false;
            if (sliding == true)
                sliding = false;
        }
    }
    void Slide()
    {
        sliding = true;
        mainVector.x += startingSlideSpeed * moveDir.x;
        canSlide = false;
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}