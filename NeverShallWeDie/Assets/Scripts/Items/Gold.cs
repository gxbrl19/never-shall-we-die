using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Gold : MonoBehaviour
{
    [SerializeField] GameObject _goldEffect;
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    [SerializeField] Collider2D _collider;
    Collider2D _trigger;

    [Header("FMOD Events")]
    [SerializeField] EventReference release;
    [SerializeField] EventReference collect;

    private void Awake()
    {
        _body = GetComponentInParent<Rigidbody2D>();
        _sprite = GetComponentInParent<SpriteRenderer>();
        _trigger = GetComponent<Collider2D>();
    }

    private void Start()
    {
        float x = Random.Range(-3f, 3f);
        float y = Random.Range(9f, 11f);
        _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);

        PlayRelease();
    }

    void StopMovement()
    {
        _body.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "AttackPoint")
        {
            GameManager.instance._gold += 1;
            PlayCollect();
            //AudioItems.instance.PlaySound(_audio._goldSound, _audio._goldVolume);
            _sprite.enabled = false;
            _collider.enabled = false;
            _trigger.enabled = false;
            Instantiate(_goldEffect, transform.position, Quaternion.identity);
            //Destroy(gameObject, 0.5f);
        }
    }

    public void PlayRelease()
    {
        RuntimeManager.PlayOneShot(release);
    }

    public void PlayCollect()
    {
        RuntimeManager.PlayOneShot(collect);
    }
}
