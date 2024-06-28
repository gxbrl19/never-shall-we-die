using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] AudioClip _breakSound;
    [SerializeField] GameObject _particleHit;
    [SerializeField] GameObject _light;
    SpriteRenderer _sprite;
    AudioSource _audio;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SwordAtk"))
        {
            _sprite.enabled = false;
            _light.SetActive(false);
            _audio.PlayOneShot(_breakSound);
            GameObject hit = Instantiate(_particleHit, transform.position, _particleHit.transform.rotation);
            Destroy(hit, 2f);
        }
    }
}
