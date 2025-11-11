using UnityEngine;
using FMODUnity;

public class Voidcaller : BossBase
{
    private enum State { Intro, Chase, Idle, Disappear, RayAttack, Octopus, Spell, Dive }
    [SerializeField] WantedBoss wantedBoss;
    private State currentState = State.Intro;
    private bool introPlayed = false;

    [Header("Chase")]
    private float attackTimer;
    private float attackCooldown = 3f;
    private Vector2 attackReturnTarget;
    private float chaseSpeed = 2f;
    private Vector2 chaseVelocity;
    private int direction;

    [Header("References")]
    private Player player;
    private Transform playerPosition;

    [Header("Ray")]
    [SerializeField] private Transform rayPrefab;
    [SerializeField] private Transform[] rayPoints;

    [Header("Octopus")]
    [SerializeField] private GameObject octopusPrefab;

    [Header("Spell")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform leftCastPoint;
    [SerializeField] private Transform rightCastPoint;

    [Header("Dive")]
    [SerializeField] private float diveDuration = 1.5f;
    [SerializeField] private float diveHeight = 5f;

    private Vector2 diveStartPos;
    private Vector2 diveTargetPos;
    private float diveTimer;

    [Header("Limits")]
    [SerializeField] private float groundY = 0f;
    [SerializeField] private float leftLimit = -15f;
    [SerializeField] private float rightLimit = 15f;

    [Header("Sequence")]
    int currentAttackIndex = 0;
    State[] attackPattern = new State[]
    {
        State.Dive,
        State.Dive,
        State.Spell,
        State.Dive,
        State.Dive,
        State.Octopus,
        State.Spell,
        State.Dive,
        State.Dive,
        State.Spell,
        State.Octopus,
        State.Dive,
        State.Dive,
        State.Disappear,
        State.Dive,
        State.Spell,
        State.Octopus,
        State.Dive,
        State.Dive,
        State.Octopus,
        State.Spell,
        State.Dive,
        State.Dive,
        State.Disappear,
    };

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
        playerPosition = player.GetComponent<Transform>();
        direction = (int)transform.localScale.x;
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
    }

    private void HandleState()
    {
        if (playerPosition == null || isDead) return;

        switch (currentState)
        {
            case State.Intro:
                break;

            case State.Chase:
                animator.SetBool("Move", false);
                attackTimer += Time.deltaTime;

                attackReturnTarget = new Vector2(
                    player.transform.position.x + (5f * -direction),
                    player.transform.position.y + 6f
                );

                Vector2 chasePos = Vector2.SmoothDamp(
                    transform.position,
                    attackReturnTarget,
                    ref chaseVelocity,
                    0.3f,
                    chaseSpeed
                );

                chasePos = ClampPosition(chasePos);
                transform.position = chasePos;

                if (attackTimer >= attackCooldown)
                    ChangeState(State.Idle);
                break;

            case State.Idle:
                rb.velocity = Vector2.zero;
                animator.SetBool("Move", false);

                if (attackTimer >= attackCooldown)
                    NextAttack();
                break;

            case State.Disappear:
                rb.velocity = Vector2.zero;
                animator.Play("Disappear");
                break;

            case State.RayAttack:
                rb.velocity = Vector2.zero;
                animator.Play("RayAttack");
                break;

            case State.Spell:
                rb.velocity = Vector2.zero;
                animator.Play("Cast");
                break;

            case State.Octopus:
                rb.velocity = Vector2.zero;
                animator.Play("OctopusAttack");
                break;

            case State.Dive:
                rb.velocity = Vector2.zero;
                animator.SetBool("Move", true);

                diveTimer += Time.deltaTime;
                float t = diveTimer / diveDuration;

                if (t >= 1f)
                {
                    FinishAttack();
                    break;
                }

                Vector2 divePos = Vector2.Lerp(diveStartPos, diveTargetPos, t);
                divePos.y += Mathf.Sin(t * Mathf.PI) * -diveHeight;

                divePos = ClampPosition(divePos);
                transform.position = divePos;
                break;
        }

        if (currentState == State.Chase)
        {
            FlipSprite();
        }
    }

    private Vector2 ClampPosition(Vector2 pos)
    {
        pos.y = Mathf.Max(pos.y, groundY);
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        return pos;
    }

    private void NextAttack()
    {
        State nextState = attackPattern[currentAttackIndex];
        currentAttackIndex++;
        if (currentAttackIndex >= attackPattern.Length)
            currentAttackIndex = 0;

        if (nextState == State.Dive)
            StartDive();
        else
            ChangeState(nextState);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    public void SpawnRays()
    {
        int emptyIndex = Random.Range(0, rayPoints.Length);

        for (int i = 0; i < rayPoints.Length; i++)
        {
            if (i == emptyIndex) continue;
            Instantiate(rayPrefab, rayPoints[i].position, Quaternion.identity);
        }
    }

    public void SpawnOctopus()
    {
        Vector3 spawnPosition = new Vector3(playerPosition.position.x, leftCastPoint.position.y);
        GameObject octopus = Instantiate(octopusPrefab, spawnPosition, Quaternion.identity);
        octopus.GetComponent<VoidcallerOctopus>().direction = direction;
    }

    public void SpawnSpell()
    {
        Vector3 spawnPosition = direction == 1
            ? new Vector3(leftCastPoint.position.x, leftCastPoint.position.y)
            : new Vector3(rightCastPoint.position.x, rightCastPoint.position.y);

        GameObject spell = Instantiate(spellPrefab, spawnPosition, Quaternion.identity);
        spell.GetComponent<VoidcallerSpell>().direction = new Vector2(direction, 0);
    }

    private void StartDive()
    {
        diveStartPos = transform.position;
        diveTargetPos = new Vector2(player.transform.position.x + (2f * direction), player.transform.position.y);
        diveTimer = 0f;
        ChangeState(State.Dive);
    }

    public void StartIntro()
    {
        if (introPlayed) return;
        introPlayed = true;
        ChangeState(State.Intro);
        animator.SetBool("Intro", true);
    }

    public void StartBoss()
    {
        ActivateBossUI();
        ChangeState(State.Chase);
        player.EnabledControls();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        BackgroundMusic.instance.MusicControl(7);
    }

    public void FinishDisappear()
    {
        ChangeState(State.RayAttack);
    }

    public void FinishAttack()
    {
        attackTimer = 0f;
        ChangeState(State.Chase);
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

    public void Dead() //chamado na animação de morte
    {
        wantedBoss.StartWanted();
    }
}
