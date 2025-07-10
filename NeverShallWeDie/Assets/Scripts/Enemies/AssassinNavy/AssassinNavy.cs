using UnityEngine;

public class AssassinNavy : MonoBehaviour
{
    [Header("Configurações de Detecção e Ataque")]
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private LayerMask _playerLayer;
    private float _attackCooldown = 2f;

    [Header("Movimentação")]
    private float _runSpeed = 5f;
    private float _dashBackForce = 5f;
    [SerializeField] private Vector2 _detectionBoxSize = new Vector2(21f, 1f);

    private Transform _playerTransform;
    private EnemyController _controller;
    private Rigidbody2D _body;

    private float _cooldownTimer = 0f;
    private bool _isOnCooldown = false;
    private bool _playerDetected = false;
    private bool _detectedEffect = false;

    private enum State
    {
        Idle,
        RunningToPlayer,
        Attacking,
        DashingBack
    }

    private State currentState = State.Idle;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _playerTransform = playerObj.transform;
        else
            Debug.LogWarning("Player não encontrado na cena!");
    }

    private void Update()
    {
        if (_controller._isDead) return;

        //atualizar animação
        _controller._animation.SetBool("Run", currentState == State.RunningToPlayer);
        _controller._animation.SetBool("Attack", currentState == State.Attacking);
        _controller._animation.SetBool("Dashback", currentState == State.DashingBack);

        Flip();

        switch (currentState)
        {
            case State.Idle:
                //parado até detectar
                break;

            case State.RunningToPlayer:
                MoveTowardPlayer();
                break;

            case State.DashingBack:
                //não faz nada, movimento acontece via física
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_controller._isDead || _playerTransform == null) return;

        DetectPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        if (currentState != State.Attacking && currentState != State.DashingBack)
        {
            if (_playerDetected && distanceToPlayer > _attackDistance)
            {
                currentState = State.RunningToPlayer;
            }
            else if (_playerDetected && distanceToPlayer <= _attackDistance && !_isOnCooldown)
            {
                currentState = State.Attacking;
                _isOnCooldown = true;
                _cooldownTimer = _attackCooldown;
                _body.velocity = Vector2.zero; //para movimento durante ataque
            }
        }

        // Atualizar cooldown
        if (_isOnCooldown)
        {
            _cooldownTimer -= Time.fixedDeltaTime;
            if (_cooldownTimer <= 0f)
                _isOnCooldown = false;
        }
    }

    private void MoveTowardPlayer()
    {
        Vector2 target = new Vector2(_playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, _runSpeed * Time.deltaTime);
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, _detectionBoxSize, 0, _playerLayer);
        _playerDetected = hit != null;

        if (hit != null && !_detectedEffect)
        {
            _controller.CreateDetectionEffect();
            _detectedEffect = true;
        }
    }

    public void FinishAttack() //chamado pela animação
    {
        if (!_controller._isDead)
        {
            StartDashBack();
        }
    }

    private void StartDashBack()
    {
        currentState = State.DashingBack;

        //aplicar impulso para trás
        float direction = transform.localScale.x > 0 ? -1f : 1f;
        _body.velocity = new Vector2(direction * _dashBackForce, _body.velocity.y);

        //parar dash após tempo
        Invoke(nameof(EndDashBack), 0.3f); //tempo da animação
    }

    private void EndDashBack()
    {
        if (_playerDetected)
            currentState = State.RunningToPlayer;
        else
            currentState = State.Idle;
    }

    private void Flip()
    {
        if (_playerTransform == null) return;

        if (currentState == State.RunningToPlayer || currentState == State.Attacking)
        {
            if (transform.position.x < _playerTransform.position.x)
                transform.localScale = new Vector2(1, 1);
            else
                transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _detectionBoxSize);
    }
}
