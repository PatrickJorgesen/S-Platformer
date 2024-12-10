using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipSprite();
    }
    void FlipSprite()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
