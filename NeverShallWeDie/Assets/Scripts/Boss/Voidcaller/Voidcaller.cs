using UnityEngine;
using FMODUnity;

public class Voidcaller : BossBase
{
    private enum State { Intro, Idle, RayAttack, Octopus, SpecialAttack, Dead }
    private State currentState = State.Intro;

    [Header("Stats")]
    private bool introPlayed = false;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float specialCooldown = 15f;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float specialTimer = 0f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    private Player player;
    private Transform playerPosition;
    private int direction;

    [Header("VoidcallerRay")]
    [SerializeField] private Transform rayPrefab;
    [SerializeField] private Transform[] rayPoints; //pontos fixos onde os raios podem cair
    [SerializeField] private GameObject octopusPrefab;
    [SerializeField] private Transform[] octopusPoints; //6ontos fixos onde o tentaculo aparecer

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

                if (attackTimer >= attackCooldown && specialTimer < specialCooldown)
                    SortAttack();
                else if (specialTimer >= specialCooldown)
                    ChangeState(State.RayAttack);
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
                specialTimer = 0f;
                break;

            case State.Dead:
                rb.velocity = Vector2.zero;
                break;
        }

        if (currentState == State.Idle)
        {
            FlipSprite();
        }
    }

    private void SortAttack()
    {
        //int attackIndex = Random.Range(0, 2);
        int attackIndex = 0;

        switch (attackIndex)
        {
            case 0:
                ChangeState(State.Octopus);
                break;
        }
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

    private void FlipSprite()
    {
        Vector2 dir = (playerPosition.position - transform.position).normalized;

        if (dir.x > 0.1f)
            direction = 1;
        else if (dir.x < -0.1f)
            direction = -1;

        transform.localScale = new Vector3(direction, 1, 1);
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
        ChangeState(State.Idle);
        player.EnabledControls();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        //BackgroundMusic.instance.MusicControl(7);
    }

    public void FinishAttack() //chamado ao final de qualquer ataque (menos rayattack)
    {
        rb.velocity = Vector2.zero;
        attackTimer = 0f;
        ChangeState(State.Idle);
    }

    public void FinishSpecialAttack() //chamado na animação RayAttack
    {
        rb.velocity = Vector2.zero;
        attackTimer = 0f;
        specialTimer = 0f;
        ChangeState(State.Idle);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(State.Dead);
    }
}
