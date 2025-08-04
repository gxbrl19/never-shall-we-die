using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Recovery : MonoBehaviour
{
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    Collider2D _collider;
    Player _player;
    PlayerHealth _health;

    [Header("FMOD Events")]
    [SerializeField] EventReference collected;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _health = _player.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        _collider.enabled = false;
        _body.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
        Invoke("EnabledCollider", 0.5f);
    }

    void EnabledCollider()
    {
        _collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            RuntimeManager.PlayOneShot(collected);
            UIManager.instance.FeedbackItem(_sprite.sprite);
            _sprite.enabled = false;
            _collider.enabled = false;
            _player.CreateRecoveryEffect();
            _health.currentMana = _health.maxMana;
            _health.SetMana(_health.currentMana);
        }
    }
}
