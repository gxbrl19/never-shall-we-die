using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamagerForTime : MonoBehaviour
{
    int attackPower = 1;
    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            playerHealth.TakeDamage(attackPower);
            UIManager.instance.FadeIn();
            Invoke("GetNextScene", .5f);
        }
    }

    void GetNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
