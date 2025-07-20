using UnityEngine;

public class Deadshot : BossBase
{
    private enum State { Intro, Idle, Run, Shotgun, SwordAttack, ThrowBomb, DashAttack, Dead }

    [Header("Stats")]
    [SerializeField] private float meleeRange = 2.5f;
    [SerializeField] private float moveSpeed = .5f;
    [SerializeField] private float swordCooldown = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attacks")]
    [SerializeField] private Transform bombPoint;
    [SerializeField] private Transform shotgunFirePoint;
    private float dashSpeed = 25f;

    [Header("Prefabs")]
    [SerializeField] private Transform bombPrefab;
    [SerializeField] private GameObject shotPrefab;

    private State currentState = State.Intro;

    [SerializeField] private float attackTimer = 0f;
    private float swordTimer = 0f;

    private bool introPlayed = false;
    private bool isGrounded = false;
    [SerializeField] private bool isDashing = false;
    private Vector2 dashDirection;
    private Player player;
    private Transform playerPosition;

    protected override void Awake()
    {
        base.Awake();

        playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
        rb.bodyType = RigidbodyType2D.Static;
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (GameManager.instance._bosses[bossId] == 1)
        {
            gameObject.SetActive(false);
            return;
        }

    }

    protected override void Update()
    {
        base.Update();

        if (isDead) return;

        HandleState();

        if (currentState != State.Intro)
        {
            attackTimer += Time.deltaTime;
            swordTimer += Time.deltaTime;
        }
    }

    private void HandleState()
    {
        if (playerPosition == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);

        switch (currentState)
        {
            case State.Intro:
                animator.SetBool("IsGrounded", isGrounded);
                break;

            case State.Idle:
                animator.SetBool("Run", false);
                break;

            case State.Run:
                MoveTowardsPlayer();
                break;

            case State.Shotgun:
                ShootShotgun();
                break;

            case State.ThrowBomb:
                BombAttack();
                break;

            case State.SwordAttack:
                SwordAttack();
                break;

            case State.DashAttack:
                DashAttack();
                break;

            case State.Dead:
                break;
        }

        if (currentState == State.Idle || currentState == State.Run)
        {
            FlipSprite();

            if (distanceToPlayer <= meleeRange && swordTimer >= swordCooldown)
            {
                ChangeState(State.SwordAttack);
            }
            else if (attackTimer >= attackCooldown)
            {
                int randomAttack = Random.Range(0, 3);

                switch (randomAttack)
                {
                    case 0:
                        ChangeState(State.Shotgun);
                        break;
                    case 1:
                        ChangeState(State.DashAttack);
                        break;
                    case 2:
                        ChangeState(State.ThrowBomb);
                        break;
                }
            }
            else
            {
                ChangeState(State.Run);
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D detectGround = Raycast(Vector2.down, 2.1f, groundLayer);
        isGrounded = detectGround;
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void MoveTowardsPlayer()
    {
        animator.SetBool("Run", true);

        Vector2 dir = (playerPosition.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    private void ShootShotgun()
    {
        rb.velocity = Vector2.zero;
        animator.Play("Shotgun");
    }

    public void Shoot() //chamado pela animação Shotgun
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        GameObject shot = Instantiate(shotPrefab, shotgunFirePoint.position, Quaternion.identity);
        shot.GetComponent<ShotgunDeadshot>().SetDirection(dir);
    }

    private void BombAttack()
    {
        rb.velocity = Vector2.zero;
        animator.Play("ThrowBomb");
    }

    public void ThrowBomb() //chamado na animação bomb
    {
        float launchForce = 1f;
        Vector2 velocity = (playerPosition.position - transform.position) * launchForce;
        Transform bomb = Instantiate(bombPrefab, bombPoint.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private void SwordAttack()
    {
        rb.velocity = Vector2.zero;
        animator.Play("SwordAttack");
        swordTimer = 0f;
    }

    private void DashAttack()
    {
        if (isDashing) return;

        rb.velocity = Vector2.zero;
        animator.Play("DashAttack");

        dashDirection = (playerPosition.position - transform.position).normalized;

        isDashing = true;
    }

    public void StartDashExecution() //chamado na animação de Dash
    {
        rb.velocity = dashDirection * dashSpeed;
    }

    private void FlipSprite()
    {
        if (isDashing) return;

        Vector2 dir = (playerPosition.position - transform.position).normalized;
        rb.velocity = dir * (moveSpeed * 6); // dash mais rápido

        if (dir.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void StartIntro() //chamado do trigger
    {
        if (introPlayed) return;

        rb.bodyType = RigidbodyType2D.Dynamic;
        introPlayed = true;
        ChangeState(State.Intro);
        animator.SetBool("Intro", true);
    }

    public void StartBoss() //chamado na animação de intro2
    {
        ActivateBossUI();
        ChangeState(State.Idle);
        player.EnabledControls();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    public void FinishAttack() //chamado nas animações de ataque
    {
        rb.velocity = Vector2.zero;
        ChangeState(State.Run);
        isDashing = false;
        attackTimer = 0f;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(State.Dead);
    }

    RaycastHit2D Raycast(Vector2 rayDirection, float length, LayerMask layerMask) //raio para detectar o chão
    {
        Vector2 point = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(point, rayDirection, length, layerMask);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(point, rayDirection * length, color);
        return hit;
    }
}
