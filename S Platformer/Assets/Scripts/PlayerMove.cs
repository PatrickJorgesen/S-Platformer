using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] ContactFilter2D groundFilter;

    bool isGrounded;
    Vector2 moveDir;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        if(moveDir.x == 1)
            rb.transform.localScale = new Vector3(1, 1, 1);
        else if(moveDir.x == -1)
            rb.transform.localScale = new Vector3(-1, 1, 1);
    }
    private void Update()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, rb.velocity.y);
    }
    private void FixedUpdate()
    {
        isGrounded = rb.IsTouching(groundFilter);
    }
    void OnJump()
    {
        Jump();
    }
    void Jump()
    {
        if(isGrounded)
            rb.velocity += new Vector2(0f, jumpForce);
    }
}
