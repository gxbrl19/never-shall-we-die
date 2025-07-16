using UnityEngine;

public class SpearNavy : EnemyBase
{
    private enum State { Idle, Chase, Throw }
    private State currentState = State.Idle;

    [Header("Comportamento")]
    private float visionRange = 12f;
    private float throwRange = 10f;
    private float moveSpeed = 3f;
    private float throwCooldown = 1.5f;

    [Header("Referências")]
    public GameObject spearPrefab;
    public Transform throwPoint;
    Transform player;

    private float throwTimer = 0f;
    private bool isThrowing = false;
    private bool playerDetected = false;

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

        if (Mathf.Abs(rb.velocity.y) > 0.1f) //bloqueia a flutuação
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        //detecta o player apenas uma vez
        if (!playerDetected && distance <= visionRange)
            playerDetected = true;

        switch (currentState)
        {
            case State.Idle:
                rb.velocity = Vector2.zero;
                animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

                if (playerDetected)
                    ChangeState(distance <= throwRange ? State.Throw : State.Chase);
                break;

            case State.Chase:
                Vector2 dir = (player.position - transform.position);
                dir.y = 0f;

                animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

                float distanceToPlayerX = Mathf.Abs(dir.x);

                //move apenas se distância for suficiente
                if (distanceToPlayerX > 0.2f)
                {
                    rb.velocity = dir.normalized * moveSpeed;

                    if (dir.x != 0)
                        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    rb.Sleep();
                }

                //se estiver próximo o suficiente, lança
                if (Vector2.Distance(transform.position, player.position) <= throwRange)
                    ChangeState(State.Throw);

                break;


            case State.Throw:
                rb.velocity = Vector2.zero;

                if (!isThrowing && throwTimer >= throwCooldown)
                {
                    animator.SetBool("Throw", true);
                    isThrowing = true;
                    throwTimer = 0f;
                }

                if (distance > throwRange)
                    ChangeState(State.Chase);
                break;
        }

        //corrige flutuação resetando a velocidade na vertical
        if (Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    private void ChangeState(State newState)
    {
        if (isDead) return;

        currentState = newState;
        isThrowing = false;
    }

    public void ThrowSpear() //chamado na animação de Throw
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
        animator.SetBool("Throw", false);
        isThrowing = false;
        ChangeState(State.Chase);
    }
}
