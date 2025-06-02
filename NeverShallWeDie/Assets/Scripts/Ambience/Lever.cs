using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Lever : MonoBehaviour
{
    bool _enabled;
    [SerializeField] Sprite _disabledSprite;
    [SerializeField] Sprite _enabledSprite;
    [SerializeField] BarrierLever _barrier;
    SpriteRenderer _sprite;
    Collider2D _collider;

    [Header("FMOD Events")]
    [SerializeField] EventReference clickSound;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (GameManager.instance._navigator == "CREW" || GameManager.instance._navigator == "COMPLETE" || GameManager.instance._navigator == "OTHER")
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_enabled && other.CompareTag("SwordAtk"))
        {
            _barrier.EnabledCamera();
            _enabled = true;
            _sprite.sprite = _enabledSprite;
            RuntimeManager.PlayOneShot(clickSound);
        }
    }
}
