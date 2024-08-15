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
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _trigger = GetComponent<Collider2D>();
    }

    private void Start()
    {
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(4f, 6f);
        _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        //Invoke("StopMovement", 0.3f);
    }

    void StopMovement()
    {
        _body.velocity = Vector2.zero;
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance._gold += 1;
            _audio.Play();
            //AudioItems.instance.PlaySound(_audio._goldSound, _audio._goldVolume);
            _sprite.enabled = false;
            _collider.enabled = false;
            Instantiate(_goldEffect, transform.position, Quaternion.identity);
            //Destroy(gameObject, 0.5f);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
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
