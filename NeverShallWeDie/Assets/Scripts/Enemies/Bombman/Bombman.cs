using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombman : MonoBehaviour
{
    float _raycastSize = 7f;
    float _launchForce = 1f;
    float _speed = 1.5f;
    bool _canAttack = true;
    bool _canWalk = true;
    bool _detectPlayer = false;
    bool _attacking = false;
    public bool _preparing = false;
    int _direction;
    EnemyController _controller;
    Transform _playerPosition;
    [SerializeField] Transform _shootPoint;
    [SerializeField] Transform _bomb;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] LayerMask _groundLayer;

    Vector2 _velocity;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    void Start()
    {
        _direction = (int)transform.localScale.x;
    }

    void Update()
    {
        if (_controller._isDead){
            IsDead();
        }

        _velocity = (_playerPosition.position - transform.position) * _launchForce;        

        if (_canAttack && _detectPlayer & !_attacking && !_preparing)
        {
            _attacking = true;
            _canAttack = false;
            _controller._animation.SetBool("Attack", true);
        }
    }

    private void FixedUpdate()
    {
        if (_controller._isDead)
            return;

        DetectPlayer();
        DetectGround();

        if (_canWalk && _preparing && !_attacking) {
            transform.Translate(new Vector3(_speed * -_direction, 0f, 0f) * Time.deltaTime);
        }
    }

    void DetectPlayer()
    {
        if (_controller._isDead)
            return;

        Vector2 _raycastPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D _hit = Physics2D.Raycast(_raycastPosition, Vector2.right * _direction, _raycastSize + 2f, _playerLayer);
        RaycastHit2D _hitBack = Physics2D.Raycast(_raycastPosition, Vector2.left * _direction, _raycastSize + 2f, _playerLayer);

        if (_hit || _hitBack)
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }

        if (_hitBack) { Flip(); }
    }

    void DetectGround() {
        if (_controller._isDead)
            return;

        Vector2 _detectGround = new Vector2(transform.position.x - (1.3f * _direction), transform.position.y);
        RaycastHit2D _hitGround = Physics2D.Raycast(_detectGround, Vector2.down, _raycastSize - 5.5f, _groundLayer);
        RaycastHit2D _hitWall = Physics2D.Raycast(_detectGround, Vector2.left * _direction, _raycastSize - 6.5f, _groundLayer);

        if (_hitGround && !_hitWall)
        {
            _canWalk = true;
        }
        else
        {
            _canWalk = false;
        }
    }

    public void Attack() //chamado na animacao shoot
    {
        Transform bomb = Instantiate(_bomb, _shootPoint.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().velocity = _velocity;
    }

    public void FinishAttack() //chamado na animacao shoot
    {
        _attacking = false;
        _preparing = true;

        if (_canWalk) {
            _controller._animation.SetBool("PreparingWalk", true);
            _controller._animation.SetBool("Attack", false);
        }
        else {
            _controller._animation.SetBool("Preparing", true);
            _controller._animation.SetBool("Attack", false);
        }
        
    }

    public void CanAttack() //chamado na animacao preparing
    {
        _preparing = false;
        _canAttack = true;        
        _controller._animation.SetBool("Preparing", false);
        _controller._animation.SetBool("PreparingWalk", false);
    }

    private void OnDrawGizmos()
    {
        Vector2 _raycastPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Debug.DrawRay(_raycastPosition, Vector2.right  * _direction * (_raycastSize  + 2f), Color.red);
        Debug.DrawRay(_raycastPosition, Vector2.left * _direction * (_raycastSize  + 2f), Color.yellow);

        Vector2 _detectGround = new Vector2(transform.position.x - (1.3f * _direction), transform.position.y);
        Debug.DrawRay(_detectGround, Vector2.down * (_raycastSize - 5.5f), Color.blue);
        Debug.DrawRay(_detectGround, Vector2.left * _direction * (_raycastSize - 6.5f), Color.cyan);
    }

    void IsDead() {
        transform.Translate(new Vector3(0f, 0f, 0f) * Time.deltaTime);
        _canWalk = false;
        _preparing = false;
        _canAttack = false;  
        _controller._animation.SetBool("Attack", false);
        _controller._animation.SetBool("Preparing", false);
        _controller._animation.SetBool("PreparingWalk", false);
    }

    void Flip()
    {
        if (_controller._isDead || _attacking)
            return;

        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        _direction *= -1;
        transform.localScale = _scale;
    }
}
