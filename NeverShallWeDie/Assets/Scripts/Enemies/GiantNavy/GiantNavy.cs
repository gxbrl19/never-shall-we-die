using UnityEngine;

public class GiantNavy : EnemyBase
{
    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;

    [Header("Stats")]
    [SerializeField] private LayerMask playerLayer;
    private float attackRange = 2.5f;
    private float moveSpeed = 2.5f;
    private float attackCooldown = 1.8f;
    private float direction;
    private float lastAttackTime;
    private bool playerDetected = false;
    Vector2 detectionBoxSize = new Vector2(10f, 1f);
    Transform player;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        animator.SetBool("Run", currentState == State.Chase);

        DetectPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (distanceToPlayer <= attackRange)
                {
                    rb.velocity = Vector2.zero;
                    ChangeState(State.Attack);
                }
                else
                {
                    MoveTowardsPlayer();
                }

                if (!playerDetected)
                    ChangeState(State.Idle);

                break;

            case State.Attack:
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;
                }

                if (distanceToPlayer > attackRange + 1f)
                {
                    ChangeState(State.Chase);
                }
                break;
        }

        FlipTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null || currentState == State.Attack) return;

        float dir = player.position.x - transform.position.x;
        if (dir != 0)
            direction = Mathf.Sign(dir);

        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    private void DetectPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (3f * transform.localScale.x), 0f); //desloca o centro para a direita (2f seria a metade)
        playerDetected = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
    }

    public void FinishAttack() //chamado na animação Attack
    {
        if (playerDetected)
            currentState = State.Chase;
        else
            currentState = State.Idle;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (2f * direction), 0f); //desloca o centro para a direita, metade do tamanho
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, detectionBoxSize);
    }
}
