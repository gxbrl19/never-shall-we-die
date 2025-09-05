using UnityEngine;
using FMODUnity;

public class Voidcaller : BossBase
{
    private enum State { Intro, Idle, Move, RayAttack, Attack2, SpecialAttack, Dead }
    private State currentState = State.Intro;

    [Header("Stats")]
    private bool introPlayed = false;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float specialCooldown = 15f;
    private float attackTimer = 0f;
    private float specialTimer = 0f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    private Player player;
    private Transform playerPosition;
    private int direction;

    [Header("VoidcallerRay")]
    [SerializeField] private Transform rayPrefab; //prefab do raio
    [SerializeField] private Transform[] rayPoints; //6 pontos fixos onde os raios podem cair

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
        playerPosition = player.GetComponent<Transform>();
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
            specialTimer += Time.deltaTime;
        }
    }

    private void HandleState()
    {
        if (playerPosition == null || isDead || player.isDead) return;

        switch (currentState)
        {
            case State.Intro:
                break;

            case State.Idle:
                rb.velocity = Vector2.zero;
                animator.SetBool("Move", false);

                // Placeholder de lógica para mudar de estado futuramente
                if (attackTimer >= attackCooldown)
                    ChangeState(State.RayAttack);
                //else if (specialTimer >= specialCooldown)
                //ChangeState(State.SpecialAttack);

                break;

            case State.Move:
                MoveTowardsPoint(); // pontos de ataque vão ser definidos por você
                break;

            case State.RayAttack:
                rb.velocity = Vector2.zero;
                animator.Play("RayAttack");
                attackTimer = 0f;
                break;

            case State.Attack2:
                rb.velocity = Vector2.zero;
                animator.Play("Attack2");
                attackTimer = 0f;
                break;

            case State.SpecialAttack:
                rb.velocity = Vector2.zero;
                animator.Play("SpecialAttack");
                attackTimer = 0f;
                specialTimer = 0f;
                break;

            case State.Dead:
                rb.velocity = Vector2.zero;
                break;
        }

        if (currentState == State.Idle || currentState == State.Move)
        {
            FlipSprite();
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void MoveTowardsPoint()
    {
        animator.SetBool("Move", true);

        // Aqui você vai definir pontos específicos para onde ele deve se mover
        // Por enquanto deixei seguindo o player como placeholder
        Vector2 dir = (playerPosition.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    public void SpawnRays() //chamado na animação
    {
        if (rayPoints.Length < 6)
        {
            Debug.LogWarning("Defina 6 pontos para o ataque de raios!");
            return;
        }

        //escolhe qual posição ficará vazia
        int emptyIndex = Random.Range(0, rayPoints.Length);

        for (int i = 0; i < rayPoints.Length; i++)
        {
            if (i == emptyIndex) continue; //espaço vazio

            Instantiate(rayPrefab, rayPoints[i].position, Quaternion.identity);
        }
    }

    private void FlipSprite()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;

        if (dir.x > 0.1f)
            direction = 1;
        else if (dir.x < -0.1f)
            direction = -1;

        transform.localScale = new Vector3(direction, 1, 1);
    }

    // Chamado pelo trigger da cutscene
    public void StartIntro()
    {
        if (introPlayed) return;

        introPlayed = true;
        ChangeState(State.Intro);
        animator.SetBool("Intro", true);
    }

    // Chamado no fim da animação de intro
    public void StartBoss()
    {
        ActivateBossUI();
        ChangeState(State.Idle);
        player.EnabledControls();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        //BackgroundMusic.instance.MusicControl(7);
    }

    // Chamado ao final de qualquer ataque
    public void FinishAttack()
    {
        rb.velocity = Vector2.zero;
        attackTimer = 0f;
        ChangeState(State.Idle);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(State.Dead);
    }
}
