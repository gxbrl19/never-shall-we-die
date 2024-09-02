using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] GameObject _goldEffect;
    AudioSource _audio;
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    [SerializeField] Collider2D _collider;
    Collider2D _trigger;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _body = GetComponentInParent<Rigidbody2D>();
        _sprite = GetComponentInParent<SpriteRenderer>();
        _trigger = GetComponent<Collider2D>();
    }

    private void Start()
    {
        float x = Random.Range(-3f, 3f);
        float y = Random.Range(9f, 11f);
        _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }

    void StopMovement()
    {
        _body.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            GameManager.instance._gold += 1;
            _audio.Play();
            //AudioItems.instance.PlaySound(_audio._goldSound, _audio._goldVolume);
            _sprite.enabled = false;
            _collider.enabled = false;
            _trigger.enabled = false;
            Instantiate(_goldEffect, transform.position, Quaternion.identity);
            //Destroy(gameObject, 0.5f);
        }
    }
}
