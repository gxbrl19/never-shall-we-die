using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : EnemyController
{
    [Header("Stats")]
    [SerializeField] private LayerMask _playerLayer;
    private float _distance;
    private float _speed = 1.3f;
    private bool _detectPlayer;
    private bool _walk;
    private bool _react;    
    [SerializeField] private bool _isAttacking;
    float _distancePlayer = 1;

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
        if (!_isDead)
        {
            if (_detectPlayer && !_walk)
            {
                if (!_react)
                {
                    //_animation.SetTrigger("Detected");
                    _react = true;
                }

                Invoke("Walk", 0.5f);
            }

            _distance = Vector2.Distance(transform.position, _player.transform.position);

            if (_detectPlayer && _walk)
            {
                if (_distance <= _distancePlayer)
                {
                    _walk = false;
                    _isAttacking = true;
                    //_animation.SetBool("Attack", true);
                    //_animation.SetBool("Move", false);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(_player.transform.position.x, _attackPoint.transform.position.y), _speed * Time.deltaTime);
                    //_animation.SetBool("Move", true);
                    //_animation.SetBool("Attack", false);
                }
            }

            ReturnToInitialPos();
        }

        Flip();
    }

    void FixedUpdate()
    {
        if (_isDead == false)
        {
            DetectPlayer();
        }
    }

    void Walk() //chamado no Invoke dentro de Update
    {
        _walk = true;
    }

    void ReturnToInitialPos()
    {
        if (!_detectPlayer)
        {
            //_animation.SetBool("Attack", false);

            if (transform.position.x != _initialPos.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, _initialPos, _speed * Time.deltaTime);
                //_animation.SetBool("Move", true);
            }
            else
            {
                //_animation.SetBool("Move", false);
            }
        }
    }

    public void DetectPlayer()
    {
        Collider2D _hit = Physics2D.OverlapBox(transform.position, _size, _angule, _playerLayer);

        if (_hit != null)
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
        if (_isAttacking || _isDead)
            return;

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

    public void FinishAttack() //chamado no fim da animação de ataque
    {
        _isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _size);
    }
}
