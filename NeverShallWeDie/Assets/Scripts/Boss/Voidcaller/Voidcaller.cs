using UnityEngine;
using FMODUnity;

public class Voidcaller : BossBase
{
    private enum State { Intro, Move, Idle, Disappear, RayAttack, Octopus, Spell, SpecialAttack, Dead }
    private State currentState = State.Intro;
    private bool introPlayed = false;

    [Header("Move")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    private float moveSpeed = 9;
    private int direction;

    [Header("References")]
    private Player player;
    private Transform playerPosition;

    [Header("Ray")]
    [SerializeField] private Transform rayPrefab;
    [SerializeField] private Transform[] rayPoints; //pontos fixos onde os raios podem cair

    [Header("Octopus")]
    [SerializeField] private GameObject octopusPrefab;

    [Header("Spell Settings")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform leftCastPoint;
    [SerializeField] private Transform rightCastPoint;

    [Header("Sequence")]
    int currentAttackIndex = 0;
    State[] attackPattern = new State[]
    {
        State.Spell,
        State.Spell,
        State.Spell,
        State.Octopus,
        State.Spell,
        State.Spell,
        State.Octopus,
        State.Disappear,
        State.Spell,
        State.Spell,
        State.Octopus,
        State.Spell,
        State.Spell,
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

        HandleState();
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
                        Flip();
                        rb.velocity = Vector2.zero;
                        ChangeState(State.Idle);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

                    if (transform.position.x <= leftLimit.position.x)
                    {
                        Flip();
                        rb.velocity = Vector2.zero;
                        ChangeState(State.Idle);
                    }
                }
                break;

            case State.Idle:
                rb.velocity = Vector2.zero;
                animator.SetBool("Move", false);
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

            case State.SpecialAttack:
                rb.velocity = Vector2.zero;
                animator.Play("SpecialAttack");
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
        Vector3 spawnPosition = new Vector3(playerPosition.position.x, leftCastPoint.position.y);
        GameObject octopus = Instantiate(octopusPrefab, spawnPosition, Quaternion.identity);
        octopus.GetComponent<VoidcallerOctopus>().direction = direction;
    }

    public void SpawnSpell() //chamado na animação
    {
        Vector3 spawnPosition = direction == 1 ? new Vector3(leftCastPoint.position.x, leftCastPoint.position.y) : new Vector3(rightCastPoint.position.x, rightCastPoint.position.y);
        GameObject spell = Instantiate(spellPrefab, spawnPosition, Quaternion.identity);
        spell.GetComponent<VoidcallerSpell>().direction.x = direction;
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

    public void FinishDisappear() //chamado na animação
    {
        ChangeState(State.RayAttack);
    }

    public void FinishAttack() //chamado ao final de qualquer ataque (menos rayattack)
    {
        ChangeState(State.Move);
    }

    private void Flip()
    {
        direction = -direction;
        transform.localScale = new Vector3(direction, 1, 1);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(State.Dead);
    }
}
