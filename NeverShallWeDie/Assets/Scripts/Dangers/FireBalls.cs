using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBalls : MonoBehaviour
{
    float _timeToFire = 170f;
    float _interval = 0f;
    [SerializeField] GameObject _fireBall;

    void FixedUpdate()
    {
        if (_interval > _timeToFire)
        {
            Instantiate(_fireBall, transform.position, transform.rotation);
            _interval = 0f;
        }

        _interval++;
    }
}
