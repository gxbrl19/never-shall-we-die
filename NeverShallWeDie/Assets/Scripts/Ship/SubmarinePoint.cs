using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinePoint : MonoBehaviour
{
    [SerializeField] Transform _nextPoint;

    bool _triggered;
    ShipOpenWorld _ship;
    ShipInput _input;

    private void Awake()
    {
        _ship = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipOpenWorld>();
        _input = _ship.GetComponent<ShipInput>();
    }

    void Update()
    {
        if (_triggered && _input.submit) //verificar também se possue a melhoria no navio
        {
            _ship._targetSubmarine = _nextPoint;
            _ship._submarine = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = false;
        }
    }
}
