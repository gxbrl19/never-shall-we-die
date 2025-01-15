using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCannon : MonoBehaviour
{
    [HideInInspector] public float _directionX;
    [HideInInspector] public float _directionY;
    private bool _destroy = false;
    private float _force = 60f;

    void FixedUpdate()
    {
        if (!_destroy)
        {
            //transform.Translate(_force * _direction * Time.deltaTime);
            transform.Translate(_directionX * _force * Time.deltaTime, _directionY * _force * Time.deltaTime, 0f);
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
