using UnityEngine;

public class Bat : EnemyBase
{
    private enum State { Idle, Chase }
    private State currentState = State.Idle;

    [Header("Stats")]
    [SerializeField] private LayerMask playerLayer;
    private float moveSpeed = 4f;
    private float direction;
    private bool playerDetected = false;
    Vector2 detectionBoxSize = new Vector2(20f, -15f);
    Transform player;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        animator.SetBool("Fly", currentState == State.Chase);

        DetectPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                MoveTowardsPlayer();
                break;
        }

        FlipTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null) return;

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
        Vector2 center = origin + new Vector2(0f, detectionBoxSize.y / (2f * transform.localScale.y)); //desloca o centro para a direita (2f seria a metade)
        playerDetected = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
    }
}
