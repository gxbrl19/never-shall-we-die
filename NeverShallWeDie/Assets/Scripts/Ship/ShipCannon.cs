using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCannon : MonoBehaviour
{
    [HideInInspector] public float _rotation;
    bool _destroy = false;
    float _force = 10f;
    Rigidbody2D _body;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!_destroy)
        {
            Vector2 engineForce = transform.up * _force;
            _body.AddForce(engineForce, ForceMode2D.Impulse);
        }
    }

    void DestroyProjectile()
    {
        _destroy = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            DestroyProjectile();
        }
    }
}
