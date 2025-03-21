using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Trampoline : MonoBehaviour
{
    private Animator _animation;
    float _jumpForce = 40f;
    [Header("FMOD Events")]
    [SerializeField] EventReference mushroom;

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

    public void JumpSound(AudioClip jumpSound) //chamado na animação
    {
        RuntimeManager.PlayOneShot(mushroom);
    }
}
