using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetboom : MonoBehaviour
{
    #region Public Variables
    public LayerMask _playerLayer;
    public float _attackDistance;
    public float _speed;
    public Vector2 _sizeBox;
    #endregion

    #region Private Variables
    EnemyController _controller;
    Transform _playerPosition;
    bool _detectPlayer = false;
    bool _explosion = false;
    #endregion

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Flip();

        if (_detectPlayer && !_explosion)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerPosition.position.x, transform.position.y), _speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        DetectPlayer();

        if (_detectPlayer)
        {
            float _hitPlayer = Vector2.Distance(transform.position, _playerPosition.position);

            if (_hitPlayer <= _attackDistance && !_explosion)
            {
                _controller._animation.SetTrigger("Explosion");
                _explosion = true;
            }
            else if (_hitPlayer > _attackDistance && !_explosion)
            {
                _controller._animation.SetBool("Run", true);
            }
        }
        else
        {
            _controller._animation.SetBool("Run", false);
        }
    }

    public void DetectPlayer()
    {
        Collider2D _hit = Physics2D.OverlapBox(transform.position, _sizeBox, 0, _playerLayer);

        if (_hit != null)
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }
    }

    void Flip()
    {
        if (!_detectPlayer)
            return;

        if (transform.position.x < _playerPosition.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (transform.position.x > _playerPosition.transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _sizeBox);
    }
}
