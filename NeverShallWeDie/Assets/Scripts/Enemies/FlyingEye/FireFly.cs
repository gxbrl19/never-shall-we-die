using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour
{
    Transform _playerAttackPoint;

    private void Awake()
    {
        _playerAttackPoint = GameObject.FindGameObjectWithTag("AttackPoint").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerAttackPoint.position.x, _playerAttackPoint.position.y + 0.2f), 5 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.gameObject.layer == 8 || other.CompareTag("SwordAtk")) 
        {
            Destroy(gameObject);
        }
    }
}
