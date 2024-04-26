using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    bool _awake;
    int _direction;
    float _distancePlayer = 10f;
    float _speed = 4f;
    float _knockbackForce = 7f;
    GameObject _attackPoint;
    Vector3 _initialPos;
    Transform _playerPos;
    EnemyController _controller;
    Rigidbody2D _body;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _initialPos = transform.position;
        _attackPoint = GameObject.FindGameObjectWithTag("AttackPoint");
        _playerPos = _attackPoint.GetComponent<Transform>();
    }

    void Update()
    {
        if (_controller._animation == null || _controller._isDead)
            return;

        if (Vector2.Distance(transform.position, _playerPos.position) < _distancePlayer) //se a distancia do inimigo é menor que a distancia determinada
        {
            _awake = true;
        }
        else
        {
            _awake = false;
        }

        if (_awake)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerPos.position.x, _playerPos.position.y + 0.2f), _speed * Time.deltaTime);
            _controller._animation.SetBool("Move", true);
        }
        else
        {
            if (transform.position != _initialPos)
            {
                transform.position = Vector2.MoveTowards(transform.position, _initialPos, _speed * Time.deltaTime);
                _controller._animation.SetBool("Move", true);
            }
            else
            {
                _controller._animation.SetBool("Move", false);
            }
        }

        Flip();
    }

    private void FixedUpdate()
    {
        Knockback();
    }

    public void Knockback()
    {
        //_body.bodyType = RigidbodyType2D.Dynamic;

        if (!_controller._onHit || _controller._isDead)
            return;

        if (_direction < 0)
        {
            _body.velocity = Vector2.zero;
            _body.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse);
        }
        else if (_direction > 0)
        {
            _body.velocity = Vector2.zero;
            _body.AddForce(Vector2.right * _knockbackForce, ForceMode2D.Impulse);
        }

        Invoke("FinishKnockback", 0.3f);
    }

    public void FinishKnockback()
    {
        // _body.bodyType = RigidbodyType2D.Kinematic;
        _body.velocity = Vector2.zero;
    }

    void Flip()
    {
        if (_awake)
        {
            if (transform.position.x > _attackPoint.transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
                _direction = 1;
            }
            else if (transform.position.x < _attackPoint.transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
                _direction = -1;
            }
        }
        else
        {
            if (transform.position.x > _initialPos.x)
            {
                transform.localScale = new Vector2(1, 1);
                _direction = 1;
            }
            else if (transform.position.x < _initialPos.x)
            {
                transform.localScale = new Vector2(-1, 1);
                _direction = -1;
            }
        }
    }
}
