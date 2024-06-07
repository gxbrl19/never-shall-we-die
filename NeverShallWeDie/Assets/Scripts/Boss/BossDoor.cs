using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] public Transform _up, _down;
    [HideInInspector] public float _speed = 2f;
    [HideInInspector] public bool _tiggered;

    AudioSource _audioSource;

    private void Awake()
    {
        //_audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        //fecha a porta quando o trigger é acionado
        if (_tiggered && transform.position.y > _down.position.y)
        {
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
        }

        //abre a porta quando o boss morre
        if (!_tiggered && transform.position.y < _up.position.y)
        {
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
        }
    }
}
