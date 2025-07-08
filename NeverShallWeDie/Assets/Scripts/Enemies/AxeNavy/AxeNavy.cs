using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AxeNavy : MonoBehaviour
{
    public List<Transform> _paths = new List<Transform>();
    [SerializeField] LayerMask _playerLayer;
    private float _speed = 2f;
    float _attackDistance = 2f;
    private int _pathIndex;
    bool _detectPlayer = false;
    bool _attack = false;
    EnemyController _controller;
    Vector2 _sizeBox = new Vector2(15f, 1f);
    Transform _playerPosition;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    void Start()
    {
        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (_controller._isDead) { return; }

        _controller._animation.SetBool("Attack", _attack);

        Flip();

        if (!_detectPlayer && !_attack)
        {
            Move();
        }
        else if (_detectPlayer && !_attack)
        {
            MoveToPlayer();
        }
    }

    void FixedUpdate()
    {
        DetectPlayer();

        if (_detectPlayer)
        {
            float _hitPlayer = Vector2.Distance(transform.position, _playerPosition.position);
            if (_hitPlayer <= _attackDistance && !_attack) { _attack = true; }
        }
    }

    void Move()
    {
        if (_controller._isDead)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _paths[_pathIndex].position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _paths[_pathIndex].position) < 0.1f)
        {
            if (_pathIndex == 0) { _pathIndex = 1; }
            else { _pathIndex = 0; }
        }
    }

    void MoveToPlayer()
    {
        if (_controller._isDead)
            return;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerPosition.position.x, transform.position.y), _speed * Time.deltaTime);
    }

    public void DetectPlayer()
    {
        Collider2D _hit = Physics2D.OverlapBox(transform.position, _sizeBox, 0, _playerLayer);

        if (_hit != null) { _detectPlayer = true; }
        else { _detectPlayer = false; }
    }

    public void FinishAttack() //chamado na animação de ataque
    {
        _attack = false;
    }

    void Flip()
    {
        if (_detectPlayer)
        {
            if (transform.position.x < _playerPosition.transform.position.x) { transform.localScale = new Vector2(1, 1); }
            else if (transform.position.x > _playerPosition.transform.position.x) { transform.localScale = new Vector2(-1, 1); }
        }
        else
        {
            Vector2 _dir = _paths[_pathIndex].position - transform.position;
            if (_dir.x > 0) { transform.localScale = new Vector2(1, 1); }
            if (_dir.x < 0) { transform.localScale = new Vector2(-1, 1); }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _sizeBox);
    }
}
