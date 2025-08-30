using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpShooter_Projectile : MonoBehaviour
{
    [HideInInspector] public float direction;
    private bool destroy = false;
    private Vector2 force = new Vector2(35f, 0f);

    void FixedUpdate()
    {
        if (!destroy)
        {
            transform.Translate(force * direction * Time.deltaTime);
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
