using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public List<Transform> _paths = new List<Transform>();
    [HideInInspector] public bool _knockback;
    private float _speed = 6f;
    private float _knockbackForce = 7f;
    private float _initialSpeed;
    private int _pathIndex;
    private int _direction;
    EnemyController _controller;
    Rigidbody2D _body;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _body = GetComponent<Rigidbody2D>();
        _initialSpeed = _speed;
    }

    void Update()
    {
        if (_controller._isDead || _controller._onHit)
        {
            _speed = 0f;
        }
        else
        {
            _speed = _initialSpeed;
            Move();
        }
    }

    private void FixedUpdate()
    {
        Knockback();
    }

    private void Move()
    {
        if (_controller._isDead || _controller._onHit)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _paths[_pathIndex].position, _speed * Time.deltaTime);
        _controller._animation.SetBool("Move", true);

        if (Vector2.Distance(transform.position, _paths[_pathIndex].position) < 0.1f)
        {
            if (_pathIndex == 0)
            {
                _pathIndex = 1;
            }
            else
            {
                _pathIndex = 0;
            }
        }

        Vector2 _dir = _paths[_pathIndex].position - transform.position;

        if (_dir.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            _direction = 1;
        }

        if (_dir.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            _direction = -1;
        }
    }

    public void Knockback()
    {
        if (!_controller._onHit || _controller._isDead)
            return;

        if (_knockback) {
            if (_direction < 0)
            {
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.right * _knockbackForce, ForceMode2D.Impulse);
            }
            else if (_direction > 0)
            {
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse);
            }

            Invoke("FinishKnockback", 0.3f);
        }
    }

    public void FinishKnockback()
    {
        _body.velocity = Vector2.zero;
        _knockback = false;
    }
}
