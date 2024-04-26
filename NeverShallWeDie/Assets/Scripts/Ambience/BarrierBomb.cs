using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBomb : MonoBehaviour
{
    Animator _animation;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "BombExplosion")
        {
            _animation.SetBool("Enabled", true);
        }
    }
}
