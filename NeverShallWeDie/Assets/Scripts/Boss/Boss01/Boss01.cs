using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    public static Boss01 instance;

    [SerializeField] BossController _bossController;
    [SerializeField] Rigidbody2D _body;

    [Header("Animation")]
    //[SerializeField] bool _isAttacking;
    [SerializeField] int _numAttack;
    [SerializeField] Vector2 _range;

    [Header("Fire")]
    [SerializeField] Animator _fireAnimation;

    [Header("Laser")]
    [SerializeField] Rigidbody2D _laser;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _speedLaser;

    [Header("Fly")]
    [SerializeField] Transform _wallCheck;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _checkRadius;
    [SerializeField] float _speedFly;
    [SerializeField] bool _isTouchingWall;

    Player _player;


    void Awake()
    {
        instance = this;

        _bossController = GetComponent<BossController>();
        _body = GetComponent<Rigidbody2D>();
        _player = FindFirstObjectByType<Player>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (_bossController._isDead) 
        {
            _body.velocity = Vector2.zero;
        }
        else
        {
            _isTouchingWall = Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _groundLayer);
            _bossController._animation.SetInteger("Attack", _numAttack);

            if (_isTouchingWall)
            {
                CancelFlyAttack();
            }
        }          
    }

    void AttackController() //chamado na anima��o de Idle
    {
        _numAttack = Random.Range((int)_range.x, (int)_range.y);        
        //_isAttacking = true;

        if(_numAttack == 1)
        {
            Invoke("FireAttack", 1f);
        }
        else if(_numAttack == 3)
        {
            FlyAttack();
        }
    }

    public void FinishAttack() //chamado nas anima��es
    {
        _numAttack = 0;
        //_isAttacking = false;
    }

    void FireAttack() //chamado no attack controller
    {
        if (_player._dead)
            return;

        _fireAnimation.SetTrigger("Start");
    }

    public void LaserAttack() //chamado na anima��o
    {
        if (_player._dead)
            return;

        Vector3 _scale = _laser.transform.localScale;
        _scale.x = _bossController._direction;
        _laser.transform.localScale = _scale;

        Rigidbody2D _shot = Instantiate(_laser, _shootPoint.position, Quaternion.identity);
        _shot.velocity = Vector2.right * _speedLaser * -_bossController._direction;
    }

    void FlyAttack()
    {
        if (!_player._dead)
        {
            _body.velocity = new Vector2(_speedFly * -_bossController._direction, 0f);
        }
        else
        {
            CancelFlyAttack();
        }        
    }

    public void CancelFlyAttack()
    {
        _numAttack = 0;
        //_isAttacking = false;
        _body.velocity = Vector2.zero;
        Flip();
    }

    void Flip()
    {
        _bossController._direction *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_wallCheck.position, _checkRadius);
    }
}
