using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPoint : MonoBehaviour
{
    public int _id;
    [SerializeField] AudioClip _destroySound;

    Animator _animation;
    AudioSource _audioSource;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameManager.instance._airCutblock[_id] == 1)
        {
            GetComponent<SpriteRenderer>().enabled = false; //substituir pela animação depois
            //_animation.SetBool("Disable", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 28) //AirCut (usar o aircut para não precisar criar outra layer)
        {
            //_animation.SetBool("Destroy", true);
            GetComponent<SpriteRenderer>().enabled = false; //substituir pela animação depois
            GameManager.instance._rocks[_id] = 1;
            //_audioSource.PlayOneShot(_destroySound);
        }
    }
}
