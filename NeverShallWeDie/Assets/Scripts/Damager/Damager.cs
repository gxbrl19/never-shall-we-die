using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int _power;
    public GameObject _hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) //Enemy
        {
            EnemyController _enemy = other.GetComponent<EnemyController>();

            if (_enemy != null)
            {
                _enemy.TakeDamage(_power);
                if (_hitEffect != null)
                {
                    Instantiate(_hitEffect, transform.position, transform.rotation);
                }
            }
        }
        else if (other.gameObject.layer == 9) //Player
        {
            PlayerHealth _playerHealth = other.GetComponent<PlayerHealth>();
            Player _player = other.GetComponent<Player>();

            if (_playerHealth != null)
            {
                //true seria o dano a direita e false o dano da esquerda
                _player._knockback = other.transform.position.x < transform.position.x ? true : false;

                _playerHealth.TakeDamage(_power);
            }
        }

        else if (other.gameObject.layer == 7) //Boss
        {
            BossController _boss = other.GetComponent<BossController>();

            if (_boss != null)
            {
                _boss.TakeDamage(_power);
                if (_hitEffect != null)
                {
                    Instantiate(_hitEffect, transform.position, transform.rotation);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 12)
        { //Enemy        
            EnemyController _enemy = other.GetComponent<EnemyController>();

            if (_enemy != null)
            {
                _enemy.TakeDamage(_power);
            }
        }
        //else if (other.gameObject.layer == 9) { //Player
        //    PlayerHealth _playerHealth = other.GetComponent<PlayerHealth>();

        //    if (_playerHealth != null) {  
        //        _playerHealth.TakeDamage(_power);
        //    }
        //}
        else if (other.gameObject.layer == 7)
        { //Boss        
            BossController _boss = other.GetComponent<BossController>();

            if (_boss != null)
            {
                _boss.TakeDamage(_power);
            }
        }
    }
}
