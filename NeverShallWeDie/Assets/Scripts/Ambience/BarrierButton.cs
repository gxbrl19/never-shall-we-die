using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierButton : MonoBehaviour
{
    public Barrier _barrier;

    [Header("Audio")]
    public AudioClip _clickSound;
    public AudioClip _upSound;

    Animator _animation;
    AudioSource _audioSource;
    
    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.gameObject.tag == "Player" || _other.gameObject.tag == "Box")
        {
            _animation.SetBool("Press", true);
            _audioSource.PlayOneShot(_clickSound);
            _barrier._press = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Player" || _other.gameObject.tag == "Box")
        {
            _animation.SetBool("Press", false);
            _audioSource.PlayOneShot(_upSound);
            _barrier._press = false;
        }
    }
}
