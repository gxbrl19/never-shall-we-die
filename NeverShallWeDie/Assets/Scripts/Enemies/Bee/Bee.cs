using UnityEngine;
using FMODUnity;

public class BeeEnemy : EnemyBase
{
    private enum State { Idle, Chase, Attack, Recover, Dead }
    private State currentState = State.Idle;

    [Header("Comportamento")]
    private float chaseSpeed = 3f;
    private float attackSpeed = 15f;
    private float knockbackForce = 7f;
    private float attackCooldown = 1.5f;
    private bool playerDetected = false;

    private Vector2 attackReturnTarget;
    private bool returningToTarget = false;


    private Vector2 detectBoxSize = new Vector2(18f, 5.5f);
    [SerializeField] private Transform detectOrigin;
    [SerializeField] private LayerMask playerLayer;

    [Header("FMOD Events")]
    [SerializeField] private EventReference attackSFX;

    private float attackTimer;
    private int direction;
    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("AttackPoint");
    }

    private void Update()
    {
        if (isDead)
        {
            rb.gravityScale = 1f;
            ChangeState(State.Dead);
            return;
        }

        HandleState();
        Flip();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                if (!playerDetected && PlayerInRange())
                {
                    playerDetected = true;
                    ChangeState(State.Chase);
                }
                break;

            case State.Chase:
                attackTimer += Time.deltaTime;

                attackReturnTarget = new Vector2(player.transform.position.x + (4f * direction), player.transform.position.y + 2.5f);
                transform.position = Vector2.MoveTowards(transform.position, attackReturnTarget, chaseSpeed * Time.deltaTime);

                if (attackTimer >= attackCooldown)
                    ChangeState(State.Attack);
                break;

            case State.Attack:
                // movimento é iniciado uma vez no método EnterAttack
                break;

            case State.Recover:
                if (!returningToTarget)
                    returningToTarget = true;

                transform.position = Vector2.MoveTowards(transform.position, attackReturnTarget, chaseSpeed * Time.deltaTime);

                float dist = Vector2.Distance(transform.position, attackReturnTarget);
                if (dist <= 0.1f)
                {
                    returningToTarget = false;
                    ChangeState(State.Chase);
                }
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        ExitState(currentState);
        EnterState(newState);
        currentState = newState;
    }

    private void EnterState(State state)
    {
        switch (state)
        {
            case State.Chase:
                attackTimer = 0f;
                break;

            case State.Attack:
                Vector3 dir = (player.transform.position - transform.position).normalized;
                rb.velocity = dir * attackSpeed;
                animator?.SetBool("Attack", true);
                RuntimeManager.PlayOneShot(attackSFX);
                Invoke(nameof(FinishAttack), 0.5f);
                break;

            case State.Recover:
                rb.velocity = Vector2.zero;
                animator?.SetBool("Attack", false);
                Invoke(nameof(EnableDamager), 0.5f);
                break;
        }
    }

    private void ExitState(State state)
    {
        if (state == State.Attack)
            rb.velocity = Vector2.zero;
    }

    private void FinishAttack()
    {
        ChangeState(State.Recover);
        attackTimer = 0f;
    }

    private void Flip()
    {
        if (player == null || currentState == State.Attack) return;

        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
            direction = 1;
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
            direction = -1;
        }
    }

    private bool PlayerInRange()
    {
        Collider2D hit = Physics2D.OverlapBox(detectOrigin.position, detectBoxSize, 0, playerLayer);
        return hit != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 8)
        {
            if (currentState == State.Attack)
                OnHit();
        }
    }

    private void EnableDamager()
    {
        if (playerDetected)
            ChangeState(State.Chase);
        else if (PlayerInRange())
        {
            playerDetected = true;
            ChangeState(State.Chase);
        }
        else
        {
            ChangeState(State.Idle);
            playerDetected = false; // opcional: só se quiser resetar
        }
    }

    private void OnHit()
    {
        FinishAttack();
        Vector2 force = direction < 0 ? Vector2.left : Vector2.right;
        rb.velocity = Vector2.zero;
        rb.AddForce(force * knockbackForce, ForceMode2D.Impulse);
        Invoke(nameof(ResetVelocity), 0.3f);
    }

    private void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(detectOrigin.position, detectBoxSize);
        }
    }
}
