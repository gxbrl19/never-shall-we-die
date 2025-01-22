using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVine : MonoBehaviour
{
    float _raycastSize = 12f;
    float _timeForAttack = 5f;
    float _timer;
    bool _detectPlayer = false;
    bool _attacking = false;
    [SerializeField] AudioClip _attackSound;
    EnemyController _controller;
    [SerializeField] Transform _shootPoint;
    [SerializeField] GameObject _projectile;
    [SerializeField] LayerMask _playerLayer;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _timer = _timeForAttack;
    }

    void Update()
    {
        if (!_attacking)
        {
            _timer += 0.1f;
        }

        if ((_timer > _timeForAttack) && _detectPlayer && !_attacking)
        {
            _timer = 0f;
            _attacking = true;
            _controller._animation.SetBool("Attack", true);
        }
    }

    void FixedUpdate()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if (_controller._isDead)
            return;

        Vector2 _raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 _raycastPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D _hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _raycastSize, _playerLayer);

        if (_hit)
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }
    }

    public void Attack() //chamado na animacao
    {
        GameObject proj = Instantiate(_projectile.gameObject, _shootPoint.position, Quaternion.identity);
        float dir = transform.localScale.x;
        proj.GetComponent<EvilVine_Projectile>()._direction = dir;
        _controller._audio.PlayOneShot(_attackSound);
    }

    public void FinishAttack() //chamado na animacao
    {
        _attacking = false;
        _controller._animation.SetBool("Attack", false);
    }

    private void OnDrawGizmos()
    {
        Vector2 _raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 _raycastPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Debug.DrawRay(_raycastPosition, _raycastDirection * _raycastSize, Color.red);
    }
}
