using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool _moveRight = true;
    private bool _moveDown = true;

    public bool _vertical;

    public float _speed = 3f;
    public Transform _pointA;
    public Transform _pointB;

    Rigidbody2D _body;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_vertical)
            MoveVertical();
        else
            MoveHorizontal();
    }

    private void MoveHorizontal()
    {
        if (transform.position.x < _pointA.position.x)
        {
            _moveRight = true;
        }
        if (transform.position.x > _pointB.position.x)
        {
            _moveRight = false;
        }

        if (_moveRight)
            transform.position = new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - _speed * Time.deltaTime, transform.position.y);
    }

    private void MoveVertical()
    {
        if (transform.position.y > _pointA.position.y)
        {
            _moveDown = true;
        }
        if (transform.position.y < _pointB.position.y)
        {
            _moveDown = false;
        }

        if (_moveDown)
            transform.position = new Vector2(transform.position.x, transform.position.y - _speed * Time.deltaTime);
        else
            transform.position = new Vector2(transform.position.x, transform.position.y + _speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D _other)
    {

    }

    private void OnCollisionExit2D(Collision2D _other)
    {

    }
}
