using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BarrelEnemy : MonoBehaviour
{
    public LayerMask _playerLayer;
    public float _speed;
    public float _raycastSize;
    public Transform _finishPoint;
    [SerializeField] GameObject _soundObject;

    bool _detectPlayer = false;
    bool _beginRoll = false;
    bool _isRolling = false;
    EnemyController _controller;
    Transform _playerPosition;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _playerPosition = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (_controller._isDead)
            return;

        _controller._audio.loop = _isRolling;

        if (_detectPlayer) { _controller._animation.SetBool("DetectPlayer", true); }
        if (_detectPlayer && !_beginRoll)
        {
            //if (!_isRolling) { PlaySound("Loop"); }
            _isRolling = true;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_finishPoint.position.x, transform.position.y), _speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        DetectPlayer();
    }

    public void FinishBeginRoll() //chamado na animação de BeginRoll
    {
        _beginRoll = false;
        _controller._animation.SetBool("DetectPlayer", false);
        _soundObject.SetActive(true);
    }

    void DetectPlayer()
    {
        if (_controller._isDead || _isRolling || _detectPlayer)
            return;

        Vector2 _raycastDirection;
        Vector2 _raycastBack;
        _raycastDirection = Vector2.right * transform.localScale.x;
        _raycastBack = Vector2.left * transform.localScale.x;
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _raycastDirection, _raycastSize, _playerLayer);
        RaycastHit2D _hitBack = Physics2D.Raycast(transform.position, _raycastBack, _raycastSize, _playerLayer);
        if (_hit || _hitBack)
        {
            _beginRoll = true;
            _detectPlayer = true;

            if (_hitBack) { Flip(); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer == 8 || other.gameObject.layer == 16) && _isRolling) //layer do ground ou katana
        {
            _isRolling = false;
            _detectPlayer = false;
            _controller._isDead = true;
            _controller._currentHealth = 0;
            _controller._animation.SetBool("Dead", true);
            _soundObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 _raycastDirection;
        _raycastDirection = Vector2.right * transform.localScale.x;
        Debug.DrawRay(transform.position, _raycastDirection * _raycastSize, Color.red);

        Vector2 _raycastBack;
        _raycastBack = Vector2.left * transform.localScale.x;
        Debug.DrawRay(transform.position, _raycastBack * _raycastSize, Color.red);
    }

    void Flip()
    {
        if (_isRolling)
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
}
