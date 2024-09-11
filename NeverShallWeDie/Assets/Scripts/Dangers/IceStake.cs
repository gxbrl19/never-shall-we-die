using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStake : MonoBehaviour
{
    Animator _animation;
    Collider2D _collider;
    Rigidbody2D _body;
    
    void Start()
    {
        _animation = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.gameObject.layer == 8) //Ground
        {
            Destroy(_collider, 0.5f);
            _body.velocity = Vector3.zero;
            _body.bodyType = RigidbodyType2D.Static;
        }
    }
}
