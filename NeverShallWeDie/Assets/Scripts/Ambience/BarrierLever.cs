using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierLever : MonoBehaviour
{
    public Transform _up, _down;
    float _speed = 1f;

    public bool _enabled;

    [Header("Audio")]
    public AudioClip _moveSound;
    public bool _audioStart;

    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_enabled && transform.position.y > _down.position.y)
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

        if (!_enabled && transform.position.y < _up.position.y)
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
