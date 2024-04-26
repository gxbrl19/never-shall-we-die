using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCut : MonoBehaviour
{
    [HideInInspector] public float _direction = 1;
    private bool _destroy = false;
    [SerializeField] private Vector2 _force;

    Animator _animation;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!_destroy)
        {
            transform.Translate(_force * _direction * Time.deltaTime);
        }
    }

    void DestroyFire()
    {
        _destroy = true;
        _animation.SetBool("Destroy", true);
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            DestroyFire();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            DestroyFire();
        }
    }
}
