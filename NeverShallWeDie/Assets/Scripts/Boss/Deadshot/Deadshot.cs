using UnityEngine;

public class Deadshot : BossBase
{
    private enum State { Intro, Idle, Run, Shotgun, SwordAttack, ThrowBomb, DashAttack, Dead }

    [Header("Comportamento")]
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private float meleeRange = 2.5f;
    [SerializeField] private float dashCooldown = 10f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float bombCooldown = 6f;
    [SerializeField] private float shotgunCooldown = 4f;

    [Header("Ataques")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform bombSpawn;
    [SerializeField] private Transform shotgunFirePoint;
    [SerializeField] private LayerMask groundLayer;

    private State currentState = State.Intro;

    private float dashTimer = 0f;
    private float bombTimer = 0f;
    private float shotgunTimer = 0f;

    private bool introPlayed = false;
    private bool isGrounded = false;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = FindObjectOfType<Player>().GetComponent<Transform>();
        rb.bodyType = RigidbodyType2D.Static;
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

        dashTimer += Time.deltaTime;
        bombTimer += Time.deltaTime;
        shotgunTimer += Time.deltaTime;
    }

    private void HandleState()
    {
        if (playerTransform == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

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
                ThrowBomb();
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
            if (distanceToPlayer <= meleeRange)
            {
                ChangeState(State.SwordAttack);
            }
            else if (dashTimer >= dashCooldown)
            {
                ChangeState(State.DashAttack);
            }
            else if (shotgunTimer >= shotgunCooldown)
            {
                ChangeState(State.Shotgun);
            }
            else if (bombTimer >= bombCooldown)
            {
                ChangeState(State.ThrowBomb);
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

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

        FlipSprite(dir.x);
    }

    private void ShootShotgun()
    {
        rb.velocity = Vector2.zero;
        animator.Play("Shotgun");
        shotgunTimer = 0f;

        // Adicione o disparo no evento da animação
    }

    private void ThrowBomb()
    {
        rb.velocity = Vector2.zero;
        animator.Play("ThrowBomb");
        bombTimer = 0f;

        if (bombPrefab != null && bombSpawn != null)
        {
            Instantiate(bombPrefab, bombSpawn.position, Quaternion.identity);
        }
    }

    private void SwordAttack()
    {
        rb.velocity = Vector2.zero;
        animator.Play("SwordAttack");

        // Coloque a lógica de dano no evento da animação
    }

    private void DashAttack()
    {
        animator.Play("DashAttack");
        dashTimer = 0f;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        rb.velocity = dir * (moveSpeed * 6); // dash mais rápido

        FlipSprite(dir.x);

        // Adicione hitbox e reset de velocidade no final do dash (evento de animação)
    }

    private void FlipSprite(float dirX)
    {
        if (dirX > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dirX < -0.1f)
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
        StartIntro();
        ActivateBossUI();
        ChangeState(State.Idle);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(State.Dead);
        rb.velocity = Vector2.zero;
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
