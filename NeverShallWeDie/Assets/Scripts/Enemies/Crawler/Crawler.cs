using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _checkPoint;
    [SerializeField] float _speed = 1;
    [SerializeField] float _nextFlip;
    [SerializeField] int _direction = 0;
    bool _onGround;    
    Vector2 _raycastPosition;
    Vector2 _raycastDirection;
    Rigidbody2D _body;
    EnemyController _controller;

    void Awake() {
        _controller = GetComponent<EnemyController>();
        _body = GetComponent<Rigidbody2D>();
    }
    
    private void Update() {
        CheckDirection();
    }

    void FixedUpdate() {
        if (_controller._isDead) {
            _body.velocity = Vector2.zero;
            return;
        }
        
        if (!_controller._onHit) { _body.velocity = transform.right * _speed; } else { _body.velocity = Vector2.zero; }
        
        _onGround = Physics2D.Raycast(_checkPoint.position, _raycastDirection, 1f, _groundLayer);

        if (!_onGround && Time.time > _nextFlip) {
            Flip();
        }
    }

    void Flip() {
        _nextFlip = Time.time + 0.25f;
        transform.Rotate(new Vector3(0, 0, -90));
        if (_direction < 3) { _direction += 1; } else { _direction = 0; }
    }

    void CheckDirection() {
        switch (_direction) {
            case 0:
                _raycastDirection = Vector2.down;
                break;
            case 1:
                _raycastDirection = Vector2.left;
                break;
            case 2:
                _raycastDirection = Vector2.up;
                break;
            case 3:
                _raycastDirection = Vector2.right;
                break;
        }
    }

    private void OnDrawGizmos() {        
        Debug.DrawRay(_checkPoint.position, _raycastDirection * 1f, Color.red);
    }
}
