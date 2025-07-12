using UnityEngine;

public class SpearNavy : EnemyBase
{
    private enum State { Idle, Chase, Throw, Hurt, Dead }
    private State currentState = State.Idle;

    [Header("Comportamento")]
    private float visionRange = 12f;
    private float throwRange = 10f;
    private float moveSpeed = 2f;
    private float throwCooldown = 1.5f;

    [Header("Referências")]
    public GameObject spearPrefab;
    public Transform throwPoint;
    Transform player;

    private float throwTimer = 0f;
    private bool isThrowing = false;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    protected override void Update()
    {
        if (isDead || isHurt || player == null) return;

        throwTimer += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                animator.Play("Idle");
                rb.velocity = Vector2.zero;

                if (distance <= visionRange)
                    ChangeState(distance <= throwRange ? State.Throw : State.Chase);
                break;

            case State.Chase:
                animator.Play("Walk");

                if (distance > visionRange)
                    ChangeState(State.Idle);
                else if (distance <= throwRange)
                    ChangeState(State.Throw);
                else
                {
                    Vector2 dir = player.position - transform.position;
                    dir.y = 0f; //resolve o bug do inimigo flutuar
                    rb.velocity = dir.normalized * moveSpeed;
                    if (dir.x != 0) transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
                }
                break;

            case State.Throw:
                rb.velocity = Vector2.zero;

                if (distance > throwRange)
                    ChangeState(State.Chase);
                else if (!isThrowing && throwTimer >= throwCooldown)
                {
                    animator.Play("Throw");
                    isThrowing = true;
                    throwTimer = 0f;
                }
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == State.Dead) return;

        currentState = newState;
        isThrowing = false;
    }

    public void ThrowSpear() // chamado por evento da animação
    {
        if (isDead || spearPrefab == null || throwPoint == null || player == null) return;

        Vector2 dir = (player.position - throwPoint.position).normalized;
        GameObject spear = Instantiate(spearPrefab, throwPoint.position, Quaternion.identity);
        spear.GetComponent<SpearProjectile>().SetDirection(dir);

        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    public void FinishThrow() //chamado na animação Throw
    {
        isThrowing = false;
        ChangeState(State.Idle); // ou Chase
    }

    public void FinishHurt() // chamado na animação Hurt
    {
        ResetHurt();
        ChangeState(State.Idle);
    }
}
