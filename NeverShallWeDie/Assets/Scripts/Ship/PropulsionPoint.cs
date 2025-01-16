using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionPoint : MonoBehaviour
{
    ShipOpenWorld _ship;
    Collider2D _collider;

    void Awake()
    {
        _ship = FindAnyObjectByType<ShipOpenWorld>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        _collider.enabled = !_ship._inPropulsion;
    }
}
