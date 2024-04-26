using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    Animator _animation;
    Collider2D _collider;
    
    void Start()
    {
        _animation = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.gameObject.layer == 8) //Ground
        {
            _animation.SetTrigger("CollGround");
            
            Destroy(gameObject, 0.7f);
        }
    }
}
