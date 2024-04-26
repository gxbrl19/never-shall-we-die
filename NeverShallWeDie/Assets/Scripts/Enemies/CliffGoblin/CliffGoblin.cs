using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffGoblin : EnemyController
{
    [Header("Stats")]
    public LayerMask _playerLayer;
    public float _distanceAtk;
    public float _distance;
    public bool _detectPlayer;
    public bool _walk;
    public bool _react;

    [Header("Collider")]
    public float _angule;
    public Vector2 _size;

    Player _player;
    GameObject _attackPoint;
    Vector3 _initialPos;

    void Start()
    {
        _initialPos = transform.position;
        _attackPoint = GameObject.FindGameObjectWithTag("AttackPoint");
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (_isDead == false)
        {
            if (_detectPlayer && !_walk)
            {
                if (!_react)
                {
                    _react = true;
                }

                Invoke("Walk", 0.5f);
            }

            _distance = Vector2.Distance(transform.position, _attackPoint.transform.position);

            if (_detectPlayer && _walk)
            {
                if (_distance <= _distanceAtk)
                {
                    _animation.SetBool("Attack", true);
                    _animation.SetBool("Move", false);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(_attackPoint.transform.position.x, _attackPoint.transform.position.y), 1.3f * Time.deltaTime);
                    _animation.SetBool("Move", true);
                    _animation.SetBool("Attack", false);
                }
            }

            Flip();
            WalkController();
        }
    }

    void FixedUpdate()
    {
        if (_isDead == false)
        {
            DetectPlayer();
        }
    }

    void Walk()
    {
        _walk = true;
    }

    void WalkController()
    {
        if (!_detectPlayer)
        {
            _animation.SetBool("Attack", false);

            if (transform.position.x != _initialPos.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, _initialPos, 1.3f * Time.deltaTime);
                _animation.SetBool("Move", true);
            }
            else
            {
                _animation.SetBool("Move", false);
            }
        }
    }

    public void DetectPlayer()
    {
        Collider2D _hit = Physics2D.OverlapBox(transform.position, _size, _angule, _playerLayer);

        if (_hit != null) //&& _player._dead == false
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
            _react = false;
            _walk = false;
        }
    }

    void Flip()
    {
        if (_detectPlayer)
        {
            if (transform.position.x < _attackPoint.transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (transform.position.x > _attackPoint.transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else
        {
            if (transform.position.x < _initialPos.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (transform.position.x > _initialPos.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _size);
    }
}
