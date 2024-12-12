using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveCap = 30;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundDeceleration = 5;
    [SerializeField] int doubleJumpAmount = 2;
    [SerializeField] float startingSlideSpeed = 1;
    [SerializeField] float slideDropOff = 0.01f;
    [SerializeField] float groundPoundSpeed = 3f;
    [SerializeField] ContactFilter2D groundFilter;

    int jumpCount = 0;
    bool sliding = false;
    bool canSlide = true;
    bool groundPound = false;
    bool isGrounded;
    bool isCrouching = false;
    Vector2 moveDir;
    Vector2 mainVector;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = new Vector2(mainVector.x, rb.velocity.y);
        if (sliding == false && isGrounded == true)
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
        else if(isGrounded == true)
        {
            if (mainVector.x > 0)
                mainVector.x -= (groundDeceleration + slideDropOff) * Time.deltaTime;
            else if (mainVector.x < 0)
                mainVector.x += (groundDeceleration + slideDropOff) * Time.deltaTime;
        }
        isGrounded = rb.IsTouching(groundFilter);
        if (isGrounded)
        {
            jumpCount = doubleJumpAmount;
            if (groundPound == true)
            {
                mainVector.y = 0;
                groundPound = false;
            }
        }
        Debug.Log(mainVector.x);
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
        canSlide = true;
        if (jumpCount > 0)
        {
            Jump();
            jumpCount--;
        }
    }
    void Crouch()
    {
        if (!isCrouching)
        { 
            if (isGrounded == true && moveDir.x != 0 && canSlide == true) // activates sliding
            {
                sliding = true;
                mainVector.x += startingSlideSpeed * moveDir.x;
                canSlide = false;
            }
            if(isGrounded == false && groundPound == false)
            {
                groundPound = true;
                mainVector.y = groundPoundSpeed;
            }
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, 0.75f, rb.transform.localScale.z);
            isCrouching = true;
        }
        else
        {
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, 1f, rb.transform.localScale.z);
            isCrouching = false;
            if(sliding == true)
                sliding = false;
        }
    }
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}