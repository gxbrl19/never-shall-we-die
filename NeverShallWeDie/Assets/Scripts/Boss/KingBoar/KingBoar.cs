using UnityEngine;
using FMODUnity;

public class KingBoar : BossBase
{
    private enum State { Intro, Idle, Walk, LaunchSpike, TrunkAttack, MinionAttack }
    [SerializeField] WantedBoss wantedBoss;
    private State currentState = State.Intro;

    [Header("Stats")]
    private bool introPlayed = false;
    private float moveSpeed = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float minionCooldown = 25f;
    private float attackTimer = 0f;
    private float minionTimer = 0f;
    [SerializeField] private float meleeRange = 4f;
    [SerializeField] private float distanceToPlayer;
    private int direction;

    [Header("Prefabs")]
    [SerializeField] private Transform spikePrefab;
    [SerializeField] private GameObject minionPrefab;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform spikePoint;
    [SerializeField] private Transform boarMinionRight;
    [SerializeField] private Transform boarMinionLeft;

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

        if (player.isDead) { ChangeState(State.Idle); }

        HandleState();

        if (currentState != State.Intro)
        {
            attackTimer += Time.deltaTime;
            minionTimer += Time.deltaTime;
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

                if (distanceToPlayer > meleeRange)
                    ChangeState(State.Walk);

                break;

            case State.Walk:
                if (distanceToPlayer <= meleeRange)
                    ChangeState(State.Idle);

                MoveTowardsPlayer();
                break;

            case State.LaunchSpike:
                rb.velocity = Vector2.zero;
                animator.Play("SpikeAttack");
                attackTimer = 0f;
                break;

            case State.TrunkAttack:
                rb.velocity = Vector2.zero;
                animator.Play("TrunkAttack");
                attackTimer = 0f;
                break;

            case State.MinionAttack:
                rb.velocity = Vector2.zero;
                animator.Play("MinionAttack");
                attackTimer = 0f;
                minionTimer = 0f;
                break;
        }

        if (currentState == State.Idle || currentState == State.Walk)
        {
            FlipSprite();

            if (minionTimer >= minionCooldown)
                ChangeState(State.MinionAttack);

            if (distanceToPlayer <= meleeRange && attackTimer >= attackCooldown)
                ChangeState(State.TrunkAttack);
            else if (distanceToPlayer > meleeRange && attackTimer >= attackCooldown)
                ChangeState(State.LaunchSpike);
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

    public void ThrowSpike() //chamado na animação spike
    {
        float launchForce = 1f;
        Vector2 velocity = (playerPosition.position - transform.position) * launchForce;

        Transform spike = Instantiate(spikePrefab, spikePoint.position, Quaternion.identity);
        spike.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x + 4f, velocity.y - 0.5f);

        Transform spike2 = Instantiate(spikePrefab, spikePoint.position, Quaternion.identity);
        spike2.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y - 0.5f);

        Transform spike3 = Instantiate(spikePrefab, spikePoint.position, Quaternion.identity);
        spike3.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x - 4f, velocity.y - 0.5f);
    }

    public void MinionAttack() //chamado na animação
    {
        if (direction == 1)
        {
            GameObject minion = Instantiate(minionPrefab, boarMinionLeft.position, Quaternion.identity);
            minion.GetComponent<BoarMinion>().minionDirection = direction;
        }
        else
        {
            GameObject minion = Instantiate(minionPrefab, boarMinionRight.position, Quaternion.identity);
            minion.GetComponent<BoarMinion>().minionDirection = direction;
        }
    }

    private void FlipSprite()
    {
        if (currentState == State.TrunkAttack) return;

        Vector2 dir = (playerPosition.position - transform.position).normalized;

        if (dir.x > 0.1f)
            direction = 1;
        else if (dir.x < -0.1f)
            direction = -1;

        transform.localScale = new Vector3(direction, 1, 1);
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
        BackgroundMusic.instance.MusicControl(7);
    }

    public void FinishAttack() //chamado nas animações de ataque
    {
        rb.velocity = Vector2.zero;
        attackTimer = 0f;
        ChangeState(State.Idle);
    }

    RaycastHit2D Raycast(Vector2 rayDirection, float length, LayerMask layerMask) //raio para detectar o chão
    {
        Vector2 point = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(point, rayDirection, length, layerMask);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(point, rayDirection * length, color);
        return hit;
    }

    public void Dead() //chamado na animação de morte
    {
        wantedBoss.StartWanted();
    }

    // SFX - chamados na animação
    public void AudioGrunt() => RuntimeManager.PlayOneShot(gruntSFX);
    public void AudioTrunk() => RuntimeManager.PlayOneShot(attackSFX);
    public void AudioSpikeThrow() => RuntimeManager.PlayOneShot(spikeThrowSFX);
    public void AudioCallBackup() => RuntimeManager.PlayOneShot(callBackupSFX);
}