using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private Animator _animation;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.gameObject.tag == "Player")
        {
            _animation.SetTrigger("StartWall");
        }
    }
}
