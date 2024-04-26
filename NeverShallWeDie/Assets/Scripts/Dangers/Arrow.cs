using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 _speed;
    [HideInInspector] public float _direction = 1;

    void Update()
    {
        transform.Translate(_speed * -_direction * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9) //Ground ou Player
        {
            Destroy(gameObject);
        }
    }
}
