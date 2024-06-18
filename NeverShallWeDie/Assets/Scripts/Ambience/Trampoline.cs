using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator _animation;
    float _jumpForce = 40f;
    AudioSource _audio;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
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

    public void JumpSound(AudioClip jumpSound) //chamado na animação
    {
        _audio.PlayOneShot(jumpSound);
    }
}
