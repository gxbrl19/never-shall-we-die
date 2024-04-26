using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Boss03 : MonoBehaviour
{
    public static Boss03 instance;

    [SerializeField] BossController _bossController;
    [SerializeField] Rigidbody2D _body;

    [Header("Animation")]
    [SerializeField] bool _isAttacking;
    [SerializeField] int _numAttack;
    [SerializeField] Vector2 _range;

    [Header("Ground Attack")]
    [SerializeField] int _nSpikes = 5;
    [SerializeField] int _spikeCountSpawn;
    [SerializeField] GameObject _spikeObject;
    [SerializeField] Transform _spikePoint;
    private Vector3 _lastSpawnPosition;

    [Header("Jump Attack")]
    [SerializeField] bool _isGrounded;
    [SerializeField] float _jumpForce;
    [SerializeField] float _groundDistance;
    [SerializeField] LayerMask _groundLayer;

    Player _player;

    void Awake()
    {
        instance = this;

        _bossController = GetComponent<BossController>();
        _body = GetComponent<Rigidbody2D>();
        _player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        Flip();

        if (_bossController._isDead)
        {
            _body.velocity = Vector2.zero;
        }
        else
        {
            _bossController._animation.SetInteger("Attack", _numAttack);
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    void AttackController() //chamado na animação de Idle
    {
        if (_bossController._isDead)
            return;

        _numAttack = Random.Range((int)_range.x, (int)_range.y);

        //testar todas as animações novamente
        _numAttack = 3;
        _isAttacking = true;

        //verificar qual é o attack para chamar o método certo
        if (_numAttack == 1)
        {
            Invoke("InvisibleAttack", 2f);
        }
    }

    public void FinishAttack() //chamado nas animações
    {
        _spikeCountSpawn = 0;
        _numAttack = 0;
        _isAttacking = false;
    }


#region Invisible
    void InvisibleAttack() //se posiciona o ficar invisível (chamado no AttackController)
    {
        transform.position = new Vector2(_player.transform.position.x, transform.position.y);
        _bossController._animation.SetTrigger("Attack1");

        if (_bossController._direction == 1)
        {
            transform.position = new Vector2(_player.transform.position.x - 0.5f, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(_player.transform.position.x + 0.5f, transform.position.y);
        }
    }
#endregion

#region GroundAttack
    public void GroundAttack() //chamado na animação
    {
        _spikeCountSpawn = 0;
        _lastSpawnPosition = _spikePoint.position;
        InvokeRepeating("GroundNextSpike", 0f, 0.1f);
    }

    void CreateSpike()
    {
        if (_bossController._direction == 1)
        {
            GameObject newObject = Instantiate(_spikeObject, _lastSpawnPosition + Vector3.left * 1f, Quaternion.identity);
            _lastSpawnPosition = newObject.transform.position;
        }
        else
        {
            GameObject newObject = Instantiate(_spikeObject, _lastSpawnPosition + Vector3.right * 1f, Quaternion.identity);
            _lastSpawnPosition = newObject.transform.position;
        }
    }

    void GroundNextSpike() //chamado no GroundAttack()
    {
        if (_spikeCountSpawn < _nSpikes)
        {
            CreateSpike();
            _spikeCountSpawn++;
        }
        else
        {
            CancelInvoke("SpawnObjectWithDelay");
            FinishAttack();
        }
    }
#endregion

#region JumpAttack
    public void Jump() 
    {
        if (!_isGrounded)
            return;

        //_body.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _body.AddForce(new Vector2((_jumpForce + 7f) * -_bossController._direction, _jumpForce + 5f), ForceMode2D.Impulse);
        _bossController._animation.SetBool("Jump", true);
    }

    public void FinishJump()
    {
        FinishAttack();
    }
#endregion

    void GroundCheck()
    {
        _isGrounded = false;

        //dispara um raio para baixo de cada pé para checagem do chão
        RaycastHit2D _ray = Raycast(Vector2.down, _groundDistance, _groundLayer);

        if (_ray)
        {
            _bossController._animation.SetBool("Jump", false);
            _isGrounded = true;            
        }
    }

    RaycastHit2D Raycast(Vector2 rayDirection, float length, LayerMask layerMask) //DISPARA UM RAIO DE COLISÃO PARA DETECTAR O CHÃO
    {
        Vector2 _bossPosition = transform.position;
        RaycastHit2D _hit = Physics2D.Raycast(_bossPosition, rayDirection, length, layerMask);
        Color _color = _hit ? Color.red : Color.green;
        Debug.DrawRay(_bossPosition, rayDirection * length, _color);
        return _hit;
    }

    void Flip()
    {
        if (_isAttacking)
            return;

        if (transform.position.x > _player.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
            _bossController._direction = 1;
        }
        else if (transform.position.x < _player.transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
            _bossController._direction = -1;
        }
    }
}
