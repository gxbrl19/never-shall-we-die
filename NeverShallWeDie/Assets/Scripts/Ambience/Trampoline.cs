using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator _animation;
    public float _jumpForce = 16.5f;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D _body = other.gameObject.GetComponent<Rigidbody2D>();
            _body.velocity = Vector2.zero;
            _body.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _animation.SetBool("Collision", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animation.SetBool("Collision", false);
        }
    }
}
