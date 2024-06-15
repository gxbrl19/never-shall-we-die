using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarMinion : MonoBehaviour
{
    [HideInInspector] public float _direction;
    Vector2 _speed = new Vector2(8f, 0f);

    void Start()
    {
        transform.localScale = new Vector2(-_direction, 1f);
    }

    void FixedUpdate()
    {
        transform.Translate(_speed * _direction * Time.deltaTime);
    }
}
