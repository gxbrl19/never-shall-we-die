using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStakes : MonoBehaviour
{
    //[SerializeField] private AudioClip _soundCollision;
    [SerializeField] private Rigidbody2D _bodyStake;

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
            _bodyStake.bodyType = RigidbodyType2D.Dynamic;
            Destroy(_collider);
            //_audio.PlayOneShot(_soundCollision);
            //Destroy(this);
        }
    }
}
