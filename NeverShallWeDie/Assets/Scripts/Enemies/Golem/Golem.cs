using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D))]
public class Golem : EnemyBase
{
    private enum State { Idle, Chase, Stunned }
    private State currentState = State.Idle;

    [Header("Golem Settings")]
    private float chaseSpeed = 15f;
    private float stunDuration = 2.5f;
    private float stunTimer;
    private float direction;
    private bool playerDetected;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 detectionBoxSize = new Vector2(13f, 1f);
    [SerializeField] private Player player;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ChangeState(State.Idle);
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        if (isDead) return;

        if (currentState != State.Stunned)
            DetectPlayer();

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("Attack", false);
                if (playerDetected)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                animator.SetBool("Attack", true);
                break;

            case State.Stunned:
                animator.SetBool("Attack", false);
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0f)
                    ChangeState(State.Idle);
                break;
        }

        FlipTowardsPlayer();
    }


    private void FixedUpdate()
    {
        if (currentState == State.Chase)
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
        else if (currentState != State.Stunned)
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == State.Chase && ((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerDetected = false;
            stunTimer = stunDuration;
            ChangeState(State.Stunned);
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
    }

    private void DetectPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (2f * transform.localScale.x), 0f); //desloca o centro para a direita (2f seria a metade)
        playerDetected = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null || currentState == State.Chase || currentState == State.Stunned) return;

        float dir = player.transform.position.x - transform.position.x;
        if (dir != 0)
            direction = Mathf.Sign(dir);

        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (2f * direction), 0f); //desloca o centro para a direita, metade do tamanho
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, detectionBoxSize);
    }
}
