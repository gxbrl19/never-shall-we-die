using UnityEngine;

public class AssassinNavy : EnemyBase
{
    private enum State { Idle, Chase, Attack, Dashback }

    [Header("Comportamento")]
    private float attackDistance = 2f;
    private float moveSpeed = 5f;
    private float attackCooldown = 2f;
    private float dashBackForce = 6f;
    private float dashBackDuration = 0.3f;

    [Header("Detecção")]
    [SerializeField] private Vector2 detectionBoxSize = new Vector2(10f, 2f);
    [SerializeField] private LayerMask playerLayer;

    private Transform player;
    private State currentState = State.Idle;
    private float cooldownTimer = 0f;
    private float dashTimer = 0f;
    private bool isOnCooldown = false;
    private bool playerDetected = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        if (isDead || isHurt || player == null) return;

        animator.SetBool("Run", currentState == State.Chase);
        animator.SetBool("Attack", currentState == State.Attack);
        animator.SetBool("Dashback", currentState == State.Dashback);

        //corrige "flutuação" se estiver com velocidade vertical e sem gravidade
        if (Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        float distance = Vector2.Distance(transform.position, player.position);

        //detectando o player
        if (!playerDetected && IsPlayerInDetectionBox())
            playerDetected = true;

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (distance <= attackDistance && !isOnCooldown)
                {
                    ChangeState(State.Attack);
                    animator.SetTrigger("Attack");
                    isOnCooldown = true;
                    cooldownTimer = attackCooldown;
                    rb.velocity = Vector2.zero;
                }
                break;

            case State.Attack:
                // Espera o evento FinishAttack()
                break;

            case State.Dashback:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    ChangeState(playerDetected ? State.Chase : State.Idle);
                }
                break;
        }

        // Cooldown de ataque
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                isOnCooldown = false;
        }

        FlipTowardsPlayer();
    }

    private void FixedUpdate()
    {
        if (isDead || isHurt || player == null) return;

        if (currentState == State.Chase)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == State.Dashback && newState != State.Idle && newState != State.Chase)
            return; // Evita interromper o dash

        currentState = newState;
    }

    public void FinishAttack() //chamado na animação de Attack
    {
        if (isDead) return;

        ChangeState(State.Dashback);
        dashTimer = dashBackDuration;

        float dashDir = transform.localScale.x > 0 ? -1 : 1;
        rb.velocity = new Vector2(dashDir * dashBackForce, rb.velocity.y);
    }

    public void FinishHurt() //chamado na animação Hurt
    {
        //animator.SetBool("Hurt", false);
        ResetHurt();
        ChangeState(State.Dashback);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null) return;

        float dir = player.position.x - transform.position.x;
        if (dir != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir), 1, 1);
    }

    private bool IsPlayerInDetectionBox()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, detectionBoxSize, 0, playerLayer);
        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }
}
