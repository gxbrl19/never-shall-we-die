using UnityEngine;
using FMODUnity;

public class KingBoar : BossBase
{
    private enum State { Intro, Idle, Walk, LaunchSpike, TrunkAttack, MinionAttack, Dead }

    [Header("Stats")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float trunkCooldown = 1.2f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float distanceToPlayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attacks")]
    [SerializeField] private Transform spikePoint;

    [Header("Prefabs")]
    [SerializeField] private Transform spikePrefab;

    private State currentState = State.Intro;

    [SerializeField] private float attackTimer = 0f;
    private float swordTimer = 0f;

    private bool introPlayed = false;
    private Player player;
    private Transform playerPosition;

    [Header("FMOD Events")]
    [SerializeField] private EventReference gruntSFX;
    [SerializeField] private EventReference attackSFX;
    [SerializeField] private EventReference spikeThrowSFX;
    [SerializeField] private EventReference callBackupSFX;

    protected override void Awake()
    {
        base.Awake();

        playerPosition = FindObjectOfType<Player>().GetComponent<Transform>();
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

        distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);

        switch (currentState)
        {
            case State.Intro:
                break;

            case State.Idle:
                rb.velocity = Vector2.zero;
                animator.SetBool("Walk", false);
                break;

            case State.Walk:
                MoveTowardsPlayer();
                break;

            case State.LaunchSpike:
                SpikeAttack();
                break;

            case State.TrunkAttack:
                TrunkAttack();
                break;

            case State.MinionAttack:
                MinionAttack();
                break;

            case State.Dead:
                break;
        }

        if (currentState == State.Idle || currentState == State.Walk)
        {
            FlipSprite();

            if (distanceToPlayer <= meleeRange)
            {
                if (swordTimer >= trunkCooldown)
                    ChangeState(State.TrunkAttack);
                else
                    ChangeState(State.Idle);
            }
            else
            {
                if (attackTimer >= attackCooldown)
                {
                    int randomAttack = Random.Range(0, 2);

                    switch (randomAttack)
                    {
                        case 0:
                            ChangeState(State.LaunchSpike);
                            break;
                        case 1:
                            ChangeState(State.MinionAttack);
                            break;
                        case 2:
                            break;
                    }
                }
                else
                {
                    ChangeState(State.Walk);
                }
            }
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void MoveTowardsPlayer()
    {
        animator.SetBool("Walk", true);

        Vector2 dir = (playerPosition.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    private void SpikeAttack()
    {
        rb.velocity = Vector2.zero;
        animator.Play("SpikeAttack");
    }

    public void ThrowSpike() //chamado na animação spike
    {
        float launchForce = 1f;
        Vector2 velocity = (playerPosition.position - transform.position) * launchForce;
        Transform bomb = Instantiate(spikePrefab, spikePoint.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private void TrunkAttack()
    {
        rb.velocity = Vector2.zero;
        animator.Play("TrunkAttack");
        swordTimer = 0f;
    }

    private void MinionAttack()
    {
        animator.Play("MinionAttack");
    }

    private void FlipSprite()
    {
        if (currentState == State.TrunkAttack) return;

        Vector2 dir = (playerPosition.position - transform.position).normalized;

        if (dir.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void StartIntro() //chamado do trigger
    {
        if (introPlayed) return;

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
        attackTimer = 0f;
        ChangeState(State.Idle);
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

    // SFX - chamados na animação
    public void AudioGrunt() => RuntimeManager.PlayOneShot(gruntSFX);
    public void AudioTrunk() => RuntimeManager.PlayOneShot(attackSFX);
    public void AudioSpikeThrow() => RuntimeManager.PlayOneShot(spikeThrowSFX);
    public void AudioCallBackup() => RuntimeManager.PlayOneShot(callBackupSFX);
}