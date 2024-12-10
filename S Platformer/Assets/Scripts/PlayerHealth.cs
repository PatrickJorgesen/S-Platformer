using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float damageInterval = 1f; // Time in seconds between each damage tick
    [SerializeField] int damageAmount = 1;
    [SerializeField] float damageTimer = 0f;
    [SerializeField] int playerHealth = 5;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            ApplyDamage();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                ApplyDamage();
                damageTimer = 0f; // Reset timer
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageTimer = 0f; // Reset timer when the player stops touching the enemy
        }
    }

    void ApplyDamage()
    {
        if (playerHealth > 0)
        {
            playerHealth -= damageAmount;
            Debug.Log("Player health: " + (playerHealth + 1));
        }
        else
            Destroy(gameObject);
    }
}
