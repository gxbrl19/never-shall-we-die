using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public static BossDoor instance;

    public Transform _up, _down;
    public float _speed = 2f;
    public bool _tiggered;

    AudioSource _audioSource;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (_tiggered && transform.position.y > _down.position.y)
        {
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
        }

        if (!_tiggered && transform.position.y < _up.position.y)
        {
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
        }
    }
}
