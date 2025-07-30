using UnityEngine;
using FMODUnity;

public class BombGoblin : EnemyBase
{
    private enum State { Idle, Attack, Preparing, Walk }

    [Header("Configurações")]
    private float launchForce = 1f;
    private float moveSpeed = 1.3f;
    private float attackCooldown = 1f;
    private float cooldownTimer = 0f;
    private int direction;
    private Vector2 boxSize = new Vector2(20f, 5f);
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform bombPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("FMOD")]
    [SerializeField] private EventReference throwBomb;

    private State currentState = State.Idle;
    private Transform player;
    private Vector2 launchVelocity;
    private bool canAttack = true;
    private bool canWalk = true;
    private bool detectPlayer = false;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        direction = (int)transform.localScale.x;
    }

    private void Update()
    {
        if (isDead) return;

        launchVelocity = (player.position - transform.position) * launchForce;

        if (!canAttack)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= attackCooldown)
            {
                canAttack = true;
                cooldownTimer = 0f;
            }
        }

        switch (currentState)
        {
            case State.Idle:
                DetectPlayer();
                if (detectPlayer && canAttack)
                {
                    currentState = State.Attack;
                    animator.SetBool("Attack", true);
                    canAttack = false;
                }
                break;

            case State.Attack:
                // aguardando animação chamar Attack()
                break;

            case State.Preparing:
                // aguardando animação chamar CanAttack()
                break;

            case State.Walk:
                if (canWalk)
                    transform.Translate(Vector2.left * direction * moveSpeed * Time.deltaTime);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        DetectGround();
    }

    private void DetectPlayer()
    {
        Vector2 boxCenter = new Vector2(transform.position.x + direction * 1.5f, transform.position.y + 0.5f); // deslocado na frente do inimigo

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, playerLayer);

        detectPlayer = hit != null;

        if (hit && hit.transform.position.x < transform.position.x && direction == 1)
            Flip();
        else if (hit && hit.transform.position.x > transform.position.x && direction == -1)
            Flip();
    }

    private void DetectGround()
    {
        Vector2 groundCheck = new Vector2(transform.position.x - (1.3f * direction), transform.position.y);
        RaycastHit2D ground = Physics2D.Raycast(groundCheck, Vector2.down, 3.5f, groundLayer);
        RaycastHit2D wall = Physics2D.Raycast(groundCheck, Vector2.left * direction, 0.5f, groundLayer);

        canWalk = ground && !wall;
    }

    // === MÉTODOS CHAMADOS NA ANIMAÇÃO ===

    public void Attack()
    {
        Transform bomb = Instantiate(bombPrefab, shootPoint.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().velocity = launchVelocity;
        RuntimeManager.PlayOneShot(throwBomb);
    }

    public void FinishAttack()
    {
        animator.SetBool("Attack", false);

        if (canWalk)
        {
            currentState = State.Walk;
            animator.SetBool("PreparingWalk", true);
        }
        else
        {
            currentState = State.Preparing;
            animator.SetBool("Preparing", true);
        }
    }

    public void CanAttack()
    {
        animator.SetBool("Preparing", false);
        animator.SetBool("PreparingWalk", false);
        currentState = State.Idle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 boxCenter = new Vector2(transform.position.x + direction * 2.5f, transform.position.y + 0.5f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    void Flip()
    {
        if (isDead || currentState == State.Attack) return;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        direction *= -1;
        transform.localScale = scale;
    }
}
