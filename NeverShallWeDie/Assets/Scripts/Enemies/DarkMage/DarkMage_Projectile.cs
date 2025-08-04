using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMage_Projectile : MonoBehaviour
{
    [HideInInspector] public float direction;
    private bool destroy = false;
    [SerializeField] private Vector2 _force;

    void FixedUpdate()
    {
        if (!destroy)
        {
            transform.Translate(_force * direction * Time.deltaTime);
        }
    }

    void DestroyProjectile()
    {
        destroy = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            DestroyProjectile();
        }
    }
}
