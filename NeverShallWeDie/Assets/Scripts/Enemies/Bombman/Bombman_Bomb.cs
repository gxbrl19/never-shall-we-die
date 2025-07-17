using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Bombman_Bomb : MonoBehaviour
{
    bool _explosion;
    float _height = 18f;
    Rigidbody2D _body;
    Animator _animation;

    [Header("FMOD Events")]
    [SerializeField] EventReference explode;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
    }

    void Start()
    {
        if (!_explosion)
        {
            _body.AddForce(Vector2.up * _height, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        if (!_explosion)
        {
            float _angle = Mathf.Atan2(_body.velocity.y, _body.velocity.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            Explosion();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            Explosion();
        }
    }

    void Explosion()
    {
        _explosion = true;
        //_body.velocity = Vector2.zero;
        _body.bodyType = RigidbodyType2D.Static;
        _animation.SetBool("Explode", true);
        Destroy(gameObject, 4f);
    }

    public void PlaySound() //chamado na animação
    {
        RuntimeManager.PlayOneShot(explode);
    }
}
