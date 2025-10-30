using UnityEngine;

public class GiantNavy : EnemyBase
{
    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;

    [Header("Stats")]
    [SerializeField] private LayerMask playerLayer;
    private float attackRange = 2f;
    private float distanceToPlayer;
    private float moveSpeed = 4f;
    private float attackCooldown = .5f;
    private float attackCounter = 0f;
    private float direction;
    private bool playerDetected = false;
    private Vector2 detectionBoxSize = new Vector2(10f, 4.5f);
    //private bool disabled = false;
    Transform player;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        //TODO: desabilitar inimigos ao derrotar o Boss que controla a mente deles
        //if (boss derrotado)
        //disabled = true;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        animator.SetBool("Run", currentState == State.Chase);

        attackCounter += .2f * Time.deltaTime;

        DetectPlayer();

        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected && attackCounter > attackCooldown)
                    ChangeState(State.Chase);

                break;

            case State.Chase:
                if (playerDetected && distanceToPlayer <= attackRange)
                    ChangeState(State.Attack);
                else if (playerDetected && distanceToPlayer > attackRange)
                    MoveTowardsPlayer();
                else
                    ChangeState(State.Idle);

                break;

            case State.Attack:
                if (attackCounter > attackCooldown)
                {
                    attackCounter = 0f;
                    rb.velocity = Vector2.zero;
                    animator.SetTrigger("Attack");
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
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (2f * direction), 0f); //desloca o centro para a direita (2f seria a metade)
        playerDetected = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
    }

    public void FinishAttack() //chamado também na animação Attack
    {
        ChangeState(State.Idle);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (2f * direction), 0f); //desloca o centro para a direita, metade do tamanho
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, detectionBoxSize);
    }
}
