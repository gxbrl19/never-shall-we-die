using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    AudioSource _audio;
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    Collider2D _collider;
    [HideInInspector] public int _keyID;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _collider.enabled = false;
        _body.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
        Invoke("EnabledCollider", 0.5f);

        switch (_keyID)
        {
            case 0:
                _sprite.color = Color.yellow;
                break;
            case 1:
                _sprite.color = Color.red;
                break;
            case 2:
                _sprite.color = Color.green;
                break;
            case 3:
                _sprite.color = Color.cyan;
                break;
            case 4:
                _sprite.color = Color.magenta;
                break;
            case 5:
                _sprite.color = Color.grey;
                break;
        }
    }

    void EnabledCollider()
    {
        _collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _audio.Play();
            _sprite.enabled = false;
            _collider.enabled = false;
            GameManager.instance._keys[_keyID] = 1;
        }
    }
}
