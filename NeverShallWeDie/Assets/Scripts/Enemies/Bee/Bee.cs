using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public LayerMask _playerLayer;
    public Vector2 _sizeBox;
    public Transform _detectPosition;

    [SerializeField] bool _detectPlayer;
    [SerializeField] bool _isAttacking;
    int _direction;
    float _speed = 3f;
    float _speedAtk = 15f;
    float _knockbackForce = 7f;
    float _timeForAttack = 2f;
    float _timeAttack = 0f;

    GameObject _attackPoint;
    EnemyController _controller;
    Rigidbody2D _body;
    [SerializeField] Collider2D _damager;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _attackPoint = GameObject.FindGameObjectWithTag("AttackPoint");
    }

    void Update()
    {
        if (_controller._isDead) { _body.gravityScale = 1f; }
        if (_controller._animation == null || _controller._isDead) { return; }

        if (_detectPlayer && !_isAttacking && _timeAttack < _timeForAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_attackPoint.transform.position.x + (4f * _direction), _attackPoint.transform.position.y + 2.5f), _speed * Time.deltaTime);
            _timeAttack += Time.deltaTime;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (_controller._isDead) { return; }

        DetectPlayer();
        Knockback();

        if (_timeAttack >= _timeForAttack && _detectPlayer) { Attack(); }
    }

    void Attack()
    {
        if (_isAttacking || _controller._isDead) { return; }

        Vector3 _pos = (_attackPoint.transform.position - transform.position).normalized;
        _body.velocity = _pos * _speedAtk;

        _controller._animation.SetBool("Attack", true);
        _isAttacking = true;
        Invoke("FinishAttack", 0.5f);
    }

    void FinishAttack()
    {
        if (!_isAttacking) { return; }

        _body.velocity = Vector2.zero;
        _controller._animation.SetBool("Attack", false);
        _isAttacking = false;
        _timeAttack = 0f;
    }

    public void Knockback()
    {
        if (!_controller._onHit) { return; }

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

    public void DetectPlayer()
    {
        if (_detectPlayer) { return; }

        _detectPlayer = false;

        Collider2D _hit = Physics2D.OverlapBox(_detectPosition.position, _sizeBox, 0, _playerLayer);

        if (_hit != null)
        {
            _detectPlayer = true;
        }
    }

    void Flip()
    {
        if (_detectPlayer && !_isAttacking)
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_detectPosition.position, _sizeBox);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9) //Player
        {
            _damager.enabled = false;
            FinishAttack();
            Invoke("EnableDamager", 0.5f);
        }
        else if (other.gameObject.layer == 8) //Ground
        {
            _damager.enabled = false;
            FinishAttack();
            Invoke("EnableDamager", 0.5f);
        }
    }

    void EnableDamager()
    {
        _damager.enabled = true;
    }
}
