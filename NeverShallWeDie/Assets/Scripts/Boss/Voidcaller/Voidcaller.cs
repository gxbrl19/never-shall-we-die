using UnityEngine;
using FMODUnity;

public class Voidcaller : BossBase
{
    private enum State { Intro, Move, Idle, RayAttack, Octopus, SpecialAttack, Dead }
    private State currentState = State.Intro;

    [Header("Stats")]
    private bool introPlayed = false;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float attackTimer = 0f;

    [Header("Move")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    private float moveSpeed = 4;
    private int direction;

    [Header("References")]
    private Player player;
    private Transform playerPosition;

    [Header("Ray")]
    [SerializeField] private Transform rayPrefab;
    [SerializeField] private Transform[] rayPoints; //pontos fixos onde os raios podem cair

    [Header("Octopus")]
    [SerializeField] private GameObject octopusPrefab;
    [SerializeField] private Transform[] octopusPoints; //6ontos fixos onde o tentaculo aparecer

    [Header("Sequence")]
    int currentAttackIndex = 0;
    State[] attackPattern = new State[]
    {
        State.Octopus,
        State.Octopus,
        State.Octopus,
        State.RayAttack,
        State.Octopus,
        State.Octopus,
        State.RayAttack,
        State.Octopus,
        State.RayAttack,
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

        HandleState();

        if (currentState != State.Intro)
        {
            attackTimer += Time.deltaTime;
        }
    }

    private void HandleState()
    {
        if (playerPosition == null || isDead || player.isDead) return;

        switch (currentState)
        {
            case State.Intro:
                break;

            case State.Move:
                animator.SetBool("Move", true);

                if (direction == 1)
                {
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

                    if (transform.position.x >= rightLimit.position.x)
                    {
                        direction = -1;
                        transform.localScale = new Vector3(direction, 1, 1);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

                    if (transform.position.x <= leftLimit.position.x)
                    {
                        direction = 1;
                        transform.localScale = new Vector3(direction, 1, 1);
                    }
                }

                if (attackTimer >= attackCooldown)
                {
                    rb.velocity = Vector2.zero;
                    ChangeState(State.Idle);
                }
                break;

            case State.Idle:
                rb.velocity = Vector2.zero;
                animator.SetBool("Move", false);

                if (attackTimer >= attackCooldown)
                    NextAttack();
                break;

            case State.RayAttack:
                rb.velocity = Vector2.zero;
                animator.Play("RayAttack");
                attackTimer = 0f;
                break;

            case State.Octopus:
                rb.velocity = Vector2.zero;
                animator.Play("OctopusAttack");
                attackTimer = 0f;
                break;

            case State.SpecialAttack:
                rb.velocity = Vector2.zero;
                animator.Play("SpecialAttack");
                attackTimer = 0f;
                break;

            case State.Dead:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    private void NextAttack()
    {
        State nextState = attackPattern[currentAttackIndex];

        currentAttackIndex++;
        if (currentAttackIndex >= attackPattern.Length)
            currentAttackIndex = 0;

        ChangeState(nextState);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    public void SpawnRays() //chamado na animação
    {
        int emptyIndex = Random.Range(0, rayPoints.Length); //escolhe qual posição ficará vazia

        for (int i = 0; i < rayPoints.Length; i++)
        {
            if (i == emptyIndex) continue; //espaço vazio
            Instantiate(rayPrefab, rayPoints[i].position, Quaternion.identity);
        }
    }

    public void SpawnOctopus() //chamado na animação
    {
        int point = Random.Range(0, octopusPoints.Length);
        Vector3 spawnPosition = new Vector3(playerPosition.position.x, octopusPoints[point].position.y);

        GameObject octopus = Instantiate(octopusPrefab, spawnPosition, Quaternion.identity);
        octopus.GetComponent<VoidcallerOctopus>().direction = direction;
    }

    public void StartIntro()
    {
        if (introPlayed) return;

        introPlayed = true;
        ChangeState(State.Intro);
        animator.SetBool("Intro", true);
    }

    public void StartBoss() //chamado no fim da animação de intro
    {
        ActivateBossUI();
        ChangeState(State.Move);
        player.EnabledControls();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        //BackgroundMusic.instance.MusicControl(7);
    }

    public void FinishAttack() //chamado ao final de qualquer ataque (menos rayattack)
    {
        attackTimer = 0f;
        ChangeState(State.Move);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(State.Dead);
    }
}
