using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool _enabled;
    [SerializeField] private Sprite _disabledSprite;
    [SerializeField] private Sprite _enabledSprite;    
    private SpriteRenderer _sprite;
    [SerializeField] private BarrierLever _barrier;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!_enabled && other.CompareTag("SwordAtk"))
        {
            _barrier._enabled = true;
            _enabled = true;
            _sprite.sprite = _enabledSprite;
        }
    }
}
