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

    [Header("FMOD Events")]
    [SerializeField] EventReference clickSound;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
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
