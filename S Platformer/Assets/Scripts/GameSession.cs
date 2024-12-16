using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerHealth = 5;

    int startHealth;

    private void Awake()
    {
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSessions > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        startHealth = playerHealth;
        Debug.Log("Player Health: " + playerHealth);
    }
    public void ProcessPlayerDamage()
    {
        if (playerHealth > 1)
        {
            ApplyDamage();
        }
        else
        {
            Dead();
        }
    }
    void ApplyDamage()
    {
        playerHealth--;
        Debug.Log("Player Health: " + playerHealth);
    }
    public void Dead()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
