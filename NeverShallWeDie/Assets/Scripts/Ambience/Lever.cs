using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool _enabled;
    [SerializeField] Sprite _disabledSprite;
    [SerializeField] Sprite _enabledSprite;
    [SerializeField] BarrierLever _barrier;
    SpriteRenderer _sprite;

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
        }
    }
}
