using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float damageInterval = 1f; // Time in seconds between each damage tick
    [SerializeField] float damageTimer = 0f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            FindFirstObjectByType<GameSession>().ProcessPlayerDamage();
        else if (collision.gameObject.CompareTag("Hazard"))
            FindFirstObjectByType<GameSession>().Dead();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f; // Reset timer
                FindFirstObjectByType<GameSession>().ProcessPlayerDamage();
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
}
