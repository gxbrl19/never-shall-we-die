using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerForTime : MonoBehaviour
{
    float _timeForDamage = 1.8f;
    bool _triggered;
    Damager _damage;
    PlayerHealth _health;

    private void Awake()
    {
        _damage = GetComponent<Damager>();
        _health = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = true;
            InvokeRepeating("SetDamage", _timeForDamage, _timeForDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = false;
            CancelInvoke("SetDamage");
        }
    }

    void SetDamage()
    {
        if (_triggered && _health._currentHealth > 0) { _health.TakeDamage(_damage._power); }
    }
}
