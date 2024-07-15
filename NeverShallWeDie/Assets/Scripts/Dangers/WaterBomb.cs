using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBomb : MonoBehaviour
{
    Animator _animation;
    AudioSource _audioSource;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _animation.SetTrigger("Explosion");
        }
    }

    public void PlaySound() //chamado na animação
    {
        _audioSource.Play();
    }
}
