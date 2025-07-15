using UnityEngine;

public class AxeNavy : EnemyBase
{
    private enum State { Idle, Patrol, Chase, Attack }

    [Header("Comportamento")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] LayerMask playerLayer;

    float patrolSpeed = 2f;
    float chaseSpeed = 3f;
    float attackDistance = 2f;
    float attackCooldown = 0.3f;
    int patrolIndex = 0;
    float cooldownTimer = 0f;
    bool playerDetected = false;
    State currentState = State.Idle;
    Vector2 detectionBoxSize = new Vector2(20f, 1f);
    Player player;

    private void Start()
    {
        currentState = State.Patrol;
        player = FindAnyObjectByType<Player>();
    }

    protected override void Update()
    {
        if (isDead || player == null) return;

        UpdateCooldown();
        DetectPlayer();

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Patrol:
                HandlePatrol();
                break;
            case State.Chase:
                HandleChase();
                break;
            case State.Attack:
                HandleAttack();
                break;
        }

        animator.SetBool("Attack", currentState == State.Attack);
        Flip();
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private void DetectPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (3f * transform.localScale.x), 0f); //desloca o centro para a direita (2f seria a metade)
        playerDetected = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
    }

    private void HandleIdle()
    {
        if (playerDetected)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }
    }

    private void HandlePatrol()
    {
        if (isHurt) return;

        if (patrolPoints.Length == 0) return;

        if (playerDetected)
        {
            currentState = State.Chase;
            return;
        }

        Transform target = patrolPoints[patrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void HandleChase()
    {
        if (isHurt) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= attackDistance && cooldownTimer <= 0f)
        {
            currentState = State.Attack;
            cooldownTimer = attackCooldown;
            rb.velocity = Vector2.zero;
            return;
        }

        if (!playerDetected)
        {
            currentState = State.Patrol;
            return;
        }

        Vector2 targetPos = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
    }

    private void HandleAttack()
    {
        if (isHurt) return;

        if (cooldownTimer <= attackCooldown - 0.5f) // tempo "aproximado" do ataque
        {
            if (playerDetected)
                currentState = State.Chase;
            else
                currentState = State.Patrol;
        }
    }

    private void Flip()
    {
        if (isHurt || currentState == State.Attack) return;

        float targetX = currentState == State.Patrol && patrolPoints.Length > 0
            ? patrolPoints[patrolIndex].position.x
            : player.transform.position.x;

        if (transform.position.x < targetX)
            transform.localScale = new Vector2(1, 1);
        else if (transform.position.x > targetX)
            transform.localScale = new Vector2(-1, 1);
    }

    public void FinishAttack() //chamado na animação de ataque
    {
        if (isDead) return;

        if (playerDetected)
            currentState = State.Chase;
        else
            currentState = State.Patrol;
    }

    public void FinishHurt() //chamado na animação Hurt
    {
        ResetHurt();

        if (playerDetected)
            currentState = State.Chase;
        else
            currentState = State.Patrol;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (6f * transform.localScale.x), 0f); //desloca o centro para a direita (2f seria a metade)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, detectionBoxSize);
    }
}
