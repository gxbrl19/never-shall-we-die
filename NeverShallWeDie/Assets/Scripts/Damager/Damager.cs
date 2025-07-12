using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int attackPower;
    public GameObject _hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) //Enemy
        {
            /*EnemyController _enemy = other.GetComponent<EnemyController>();

            if (_enemy != null)
            {
                _enemy.TakeDamage(attackPower);
                if (_hitEffect != null)
                {
                    Instantiate(_hitEffect, transform.position, transform.rotation);
                }
            }*/

            Transform playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null)
            {
                Vector2 dir = (other.transform.position - playerPosition.position).normalized;
                enemy.TakeHit(attackPower, dir, 6f); //knockback + dano
                Instantiate(_hitEffect, transform.position, transform.rotation);
            }
        }
        else if (other.gameObject.layer == 9) //Player
        {
            PlayerHealth _playerHealth = other.GetComponent<PlayerHealth>();
            Player _player = other.GetComponent<Player>();

            if (_playerHealth != null)
            {
                Vector2 dir = (transform.position - other.transform.position).normalized;
                _player._knockbackDirection = dir.normalized.x;
                _playerHealth.TakeDamage(attackPower);
            }
        }

        else if (other.gameObject.layer == 7) //Boss
        {
            BossController _boss = other.GetComponent<BossController>();

            if (_boss != null)
            {
                _boss.TakeDamage(attackPower);
                if (_hitEffect != null)
                {
                    Instantiate(_hitEffect, transform.position, transform.rotation);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) //Enemy
        {
            EnemyController _enemy = other.GetComponent<EnemyController>();

            if (_enemy != null)
            {
                _enemy.TakeDamage(attackPower);
            }
        }
        //else if (other.gameObject.layer == 9) { //Player
        //    PlayerHealth _playerHealth = other.GetComponent<PlayerHealth>();

        //    if (_playerHealth != null) {  
        //        _playerHealth.TakeDamage(_power);
        //    }
        //}
        else if (other.gameObject.layer == 7) //Boss
        {
            BossController _boss = other.GetComponent<BossController>();

            if (_boss != null)
            {
                _boss.TakeDamage(attackPower);
            }
        }
    }
}
