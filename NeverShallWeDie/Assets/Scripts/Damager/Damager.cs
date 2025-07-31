using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int attackPower;
    public GameObject hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) //Enemy
        {
            Transform playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null)
            {
                Vector2 dir = (other.transform.position - playerPosition.position).normalized;
                enemy.TakeHit(attackPower, dir, 7f); //knockback + dano
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
        }
        else if (other.gameObject.layer == 9) //Player
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackPower);
            }
        }

        else if (other.gameObject.layer == 7) //Boss
        {
            Transform playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
            IBoss boss = other.GetComponent<IBoss>();
            if (boss != null)
            {
                Vector2 dir = (other.transform.position - playerPosition.position).normalized;
                boss.TakeHit(attackPower, dir, 6f); //knockback + dano
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
        }
    }
}
