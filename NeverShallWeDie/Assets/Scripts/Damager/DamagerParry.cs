using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerParry : MonoBehaviour
{
    public int attackPower;
    public GameObject hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 0) //Default (Parry)
        {
            Player player = FindObjectOfType<Player>().GetComponent<Player>();
            Transform playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
            IEnemy enemy = GetComponentInParent<IEnemy>();

            if (player.isRolling && enemy != null)
            {
                Vector2 dir = (other.transform.position - playerPosition.position).normalized;
                enemy.TakeHit(0, dir, 7f); //knockback + dano
            }
        }

        else if (other.gameObject.layer == 9) //Player
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
                playerHealth.TakeDamage(attackPower);
        }
    }
}
