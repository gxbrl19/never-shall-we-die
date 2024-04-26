using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    AudioSource _audio;
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    Collider2D _collider;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(3f, 6f);
        _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        //Invoke("StopMovement", 0.3f);
    }

    void StopMovement()
    {
        _body.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //int _value = Random.Range(8, 12);
            GameManager.instance._gold += 1;
            //LevelController.instance._goldInLevel += _value;
            _audio.Play();
            //AudioItems.instance.PlaySound(_audio._goldSound, _audio._goldVolume);
            _sprite.enabled = false;
            _collider.enabled = false;
            //Destroy(gameObject, 0.5f);
        }
    }
}
