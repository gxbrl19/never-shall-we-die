using UnityEngine;

public class AssassinNavy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Move")]
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float dashBackForce = 5f;
    [SerializeField] private Vector2 detectionBoxSize = new Vector2(10f, 2f);

    private Transform playerTransform;
    private EnemyController _controller;
    private Rigidbody2D _body;

    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;
    private bool playerDetected = false;

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
            playerTransform = playerObj.transform;
        else
            Debug.LogWarning("Player não encontrado na cena!");
    }

    private void Update()
    {
        if (_controller._isDead) return;

        _controller._animation.SetBool("Run", currentState == State.RunningToPlayer);
        _controller._animation.SetBool("Attack", currentState == State.Attacking);
        _controller._animation.SetBool("Dashback", currentState == State.DashingBack);

        Flip();
    }

    private void FixedUpdate()
    {
        if (_controller._isDead || playerTransform == null) return;

        DetectPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected)
                    currentState = distanceToPlayer <= attackDistance && !isOnCooldown ? State.Attacking : State.RunningToPlayer;
                break;

            case State.RunningToPlayer:
                if (distanceToPlayer <= attackDistance && !isOnCooldown)
                {
                    currentState = State.Attacking;
                    isOnCooldown = true;
                    cooldownTimer = attackCooldown;
                    _body.velocity = Vector2.zero;
                }
                else
                {
                    MoveTowardPlayer(); // agora usa MovePosition
                }
                break;

            case State.Attacking:
                // movimento ocorre via animação
                break;

            case State.DashingBack:
                // movimento via impulso
                break;
        }

        // Atualizar cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.fixedDeltaTime;
            if (cooldownTimer <= 0f)
                isOnCooldown = false;
        }
    }

    private void MoveTowardPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Vector2 targetPosition = _body.position + direction * runSpeed * Time.fixedDeltaTime;
        _body.MovePosition(targetPosition);
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, detectionBoxSize, 0, playerLayer);
        playerDetected = hit != null;
    }

    public void FinishAttack() // chamado pela animação
    {
        if (!_controller._isDead)
        {
            StartDashBack();
        }
    }

    private void StartDashBack()
    {
        currentState = State.DashingBack;

        float direction = transform.localScale.x > 0 ? -1f : 1f;
        _body.velocity = new Vector2(direction * dashBackForce, _body.velocity.y);

        Invoke(nameof(EndDashBack), 0.3f); // ajustar tempo conforme a animação
    }

    private void EndDashBack()
    {
        if (_controller._isDead) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (playerDetected && distance > attackDistance)
            currentState = State.RunningToPlayer;
        else
            currentState = State.Idle;
    }

    private void Flip()
    {
        if (playerTransform == null) return;

        if (currentState == State.RunningToPlayer || currentState == State.Attacking)
        {
            if (transform.position.x < playerTransform.position.x)
                transform.localScale = new Vector2(1, 1);
            else
                transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }
}
