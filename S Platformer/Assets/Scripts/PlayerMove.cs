using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int doubleJumpAmount = 2;
    [SerializeField] float startingSlideSpeed = 1;
    [SerializeField] float slideDropOff = 0.01f;
    [SerializeField] ContactFilter2D groundFilter;

    float curentSlidingSpeed = 1;
    int jumpCount = 0; 
    bool sliding = false;
    bool isGrounded;
    bool isCrouching = false;
    Vector2 moveDir;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (sliding == false)
        {
            rb.velocity = new Vector2(moveDir.x * moveSpeed, rb.velocity.y);
            curentSlidingSpeed = startingSlideSpeed;
        }
        else
        {
            rb.velocity = new Vector2(moveDir.x * curentSlidingSpeed, rb.velocity.y);
            if(curentSlidingSpeed > 0)
                curentSlidingSpeed -= slideDropOff;
        }
        isGrounded = rb.IsTouching(groundFilter);
        if (isGrounded)
            jumpCount = doubleJumpAmount;
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
    void OnCrouch()
    {
        Crouch();
    }
    void OnJump()
    {
        if(jumpCount > 0)
            Jump();
    }
    void Crouch()
    {
        if (!isCrouching)
        { 
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, 0.75f, rb.transform.localScale.z);
            isCrouching = true;
            if (isGrounded && moveDir.x != 0)
                Slide();
        }
        else
        {
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, 1f, rb.transform.localScale.z);
            isCrouching = false;
            Slide();
        }
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount--;
    }
    void Slide()
    {
        sliding = !sliding;
    }
}