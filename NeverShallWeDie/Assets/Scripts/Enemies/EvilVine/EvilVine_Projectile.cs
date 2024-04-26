using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVine_Projectile : MonoBehaviour
{
    [HideInInspector] public float _direction;
    private bool _destroy = false;
    [SerializeField] private Vector2 _force;

    void FixedUpdate() {
        if (!_destroy) {
            transform.Translate(_force * _direction * Time.deltaTime);  
        }              
    }

    void DestroyProjectile() {
        _destroy = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9) {
            DestroyProjectile();
        }            
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9) {
            DestroyProjectile();
        }
    }
}
