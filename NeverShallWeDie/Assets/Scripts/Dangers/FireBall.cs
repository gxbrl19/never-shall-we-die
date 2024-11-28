using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    float _force = 12f;
    Rigidbody2D _body;
    int _direction = 1;
    Vector3 _scale;

    void Awake()
    {
        _scale = transform.localScale;
        _body = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _body.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
        Destroy(gameObject, 2.5f);
    }

    void Update()
    {
        if (_body.velocity.y > 0 && _direction == -1)
        {
            _direction = 1;
            _scale.y *= _direction;
        }
        else if (_body.velocity.y < 0 && _direction == 1)
        {
            _direction = -1;
            _scale.y *= _direction;
        }

        transform.localScale = _scale;
    }
}
