using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GateCrank : MonoBehaviour
{
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    Collider2D _collider;

    [Header("FMOD Events")]
    [SerializeField] EventReference collected;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _collider.enabled = false;
        _body.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
        Invoke("EnabledCollider", 0.5f);
    }

    void EnabledCollider() //Invoke
    {
        _collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            RuntimeManager.PlayOneShot(collected);
            _sprite.enabled = false;
            _collider.enabled = false;
            GameManager.instance._gateMechanism = 1;
        }
    }
}
