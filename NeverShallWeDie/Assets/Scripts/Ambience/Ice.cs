using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    Animator _animation;
    Player _player;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _player = FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bomb")
        {
            _animation.SetTrigger("Fire");           
        }
    }
}
