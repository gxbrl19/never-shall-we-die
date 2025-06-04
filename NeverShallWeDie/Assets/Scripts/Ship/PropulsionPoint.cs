using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionPoint : MonoBehaviour
{
    ShipOpenWorld _ship;
    Collider2D _collider;
    ShipInput _input;

    void Awake()
    {
        _ship = FindAnyObjectByType<ShipOpenWorld>();
        _input = _ship.gameObject.GetComponent<ShipInput>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        _collider.enabled = !_input.propulsion;
    }
}
