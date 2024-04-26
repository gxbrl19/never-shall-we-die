using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public static Barrier instance;

    public Transform _up, _down;
    public float _speed = 2f;

    public bool _press;    

    [Header("Audio")]
    public AudioClip _moveSound;
    public bool _audioStart;

    AudioSource _audioSource;

    private void Awake()
    {
        instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {       
        if (_press && transform.position.y > _down.position.y)
        {
            //_audioStart = true;
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
            //AudioMove();
            _audioSource.PlayOneShot(_moveSound);

            if (transform.position.y <= _down.position.y)
            {
                _audioSource.Stop();
            }
        }
        
        if(!_press && transform.position.y < _up.position.y)
        {
            //_audioStart = true;
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
            _audioSource.PlayOneShot(_moveSound);
            //AudioMove();

            if (transform.position.y >= _up.position.y)
            {
                _audioSource.Stop();
            }
        }
    }

    public void AudioMove()
    {
        if (_audioStart)
        {

            _audioStart = false;
        }
    }
}
