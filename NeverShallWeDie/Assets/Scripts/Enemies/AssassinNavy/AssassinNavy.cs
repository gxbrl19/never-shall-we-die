using UnityEngine;

public class AssassinNavy : EnemyBase
{
    private enum State { Idle, Chase, Attack, Dashback }

    [Header("Comportamento")]
    private float attackDistance = 2f;
    private float moveSpeed = 5f;
    private float attackCooldown = 1f;
    private float dashBackForce = 6f;
    private float dashBackDuration = .3f;
    private float direction;

    [Header("Detecção")]
    [SerializeField] private Vector2 detectionSize = new Vector2(10f, 2f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    private Transform player;
    private State currentState = State.Idle;
    private float cooldownTimer = 0f;
    private float dashTimer = 0f;
    private bool isGrounded = false;
    private bool isOnCooldown = false;
    private bool playerDetected = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isDead || isHurt || player == null) return;

        animator.SetBool("Dashback", currentState == State.Dashback);
        animator.SetBool("Run", currentState == State.Chase);

        float distance = Vector2.Distance(transform.position, player.position);

        //detectando o player
        playerDetected = IsPlayerInDetectionBox();

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (distance <= attackDistance)
                {
                    if (!isOnCooldown)
                    {
                        ChangeState(State.Attack);
                        animator.SetTrigger("Attack");
                        isOnCooldown = true;
                        cooldownTimer = attackCooldown;
                        rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        ChangeState(State.Idle);
                    }
                }

                if (!playerDetected)
                    ChangeState(State.Idle);
                break;

            case State.Dashback:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    ChangeState(State.Chase);
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
        if (isDead || player == null) return;

        RaycastHit2D detectGround = Raycast(Vector2.down, 1.1f, groundLayer);
        isGrounded = detectGround;

        if (currentState == State.Chase && isGrounded && playerDetected)
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

    private void FlipTowardsPlayer()
    {
        if (player == null) return;

        float dir = player.position.x - transform.position.x;
        if (dir != 0)
            direction = Mathf.Sign(dir);

        transform.localScale = new Vector3(direction, 1, 1);
    }

    private bool IsPlayerInDetectionBox()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionSize.x / (2f * direction), 0f); //desloca o centro para a direita, metade do tamanho
        Collider2D hit = Physics2D.OverlapBox(center, detectionSize, 0, playerLayer);
        return hit != null;
    }

    RaycastHit2D Raycast(Vector2 rayDirection, float length, LayerMask layerMask) //raio para detectar o chão
    {
        Vector2 point = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(point, rayDirection, length, layerMask);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(point, rayDirection * length, color);
        return hit;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionSize.x / (2f * direction), 0f); //desloca o centro para a direita, metade do tamanho
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, detectionSize);
    }
}
