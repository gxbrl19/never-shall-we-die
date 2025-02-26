using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BossDoor : MonoBehaviour
{
    [SerializeField] public Transform _up, _down;
    [HideInInspector] public float _speed = 2f;
    [HideInInspector] public bool _tiggered;

    [Header("FMOD Events")]
    [SerializeField] EventReference portalClose;
    bool _start = false;

    public void EnabledSound()
    {
        if (_start) { return; }
        RuntimeManager.PlayOneShot(portalClose);
        _start = true;
    }

    void Update()
    {
        //fecha a porta quando o trigger é acionado
        if (_tiggered && transform.position.y > _down.position.y)
        {
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
            EnabledSound();
        }

        //abre a porta quando o boss morre
        if (!_tiggered && transform.position.y < _up.position.y)
        {
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
            EnabledSound();
        }
    }
}
