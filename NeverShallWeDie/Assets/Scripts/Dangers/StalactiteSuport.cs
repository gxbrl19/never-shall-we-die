using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteSuport : MonoBehaviour
{
    [SerializeField] private AudioClip _soundBreak;
    [SerializeField] private Rigidbody2D _bodyStalactite;

    AudioSource _audio;
    Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            Destroy(_collider);
            _audio.PlayOneShot(_soundBreak);
            Destroy(this);
            _bodyStalactite.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
