using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public List<Transform> _paths = new List<Transform>();
    [SerializeField] private float _speed;
    private float _initialSpeed;
    private int _pathIndex;
    //private int _direction;
    EnemyController _controller;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
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
            //_direction = 1;
        }

        if (_dir.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            //_direction = -1;
        }
    }
}
