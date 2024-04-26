using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public GameObject _cameraPlayer;
    private float _lenght, _startPos;
    public float _speedParallax;
    void Start()
    {
        _startPos = transform.position.x;
        _lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float _temp = (_cameraPlayer.transform.position.x * (1 - _speedParallax));
        float _distance = (_cameraPlayer.transform.position.x * _speedParallax);

        transform.position = new Vector3(_distance, transform.position.y, transform.position.z);

        /*if(_temp > _startPos + _lenght / 2)
        {
            _startPos += _lenght;
        }
        else if (_temp < _startPos - _lenght / 2)
        {
            _startPos -= _lenght;
        }*/
    }
}
