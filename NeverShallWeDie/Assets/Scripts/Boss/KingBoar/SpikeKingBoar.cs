using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeKingBoar : MonoBehaviour
{
    float _height = 18f;
    Rigidbody2D _body;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _body.AddForce(Vector2.up * _height, ForceMode2D.Impulse);
    }

    void Update()
    {
        float _angle = Mathf.Atan2(_body.velocity.y, _body.velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }
}
