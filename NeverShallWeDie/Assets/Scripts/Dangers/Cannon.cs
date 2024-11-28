using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private CannonBall _ball;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _offsetBox;
    [SerializeField] private Vector2 _sizeBox;
    [SerializeField] private LayerMask _playerLayer;

    private bool _enabled = false;
    private Animator _animation;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _animation.SetBool("Enabled", _enabled);
        bool _playerTriggered = Physics2D.OverlapBox(new Vector2(transform.position.x + _offsetBox, transform.position.y), _sizeBox, 0, _playerLayer);
        if (_playerTriggered) { _enabled = true; } else { _enabled = false; }
    }

    public void Fire()
    {
        _ball._direction = transform.localScale.x;

        Vector3 _scale = _ball.transform.localScale;
        _scale.x = transform.localScale.x;
        _ball.transform.localScale = _scale;

        Instantiate(_ball.gameObject, _shootPoint.position, _shootPoint.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + _offsetBox, transform.position.y), _sizeBox);
    }
}
