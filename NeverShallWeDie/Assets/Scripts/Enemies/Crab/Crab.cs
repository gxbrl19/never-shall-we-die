using UnityEngine;

public class Crab : EnemyBase
{
    private enum State { Idle, Patrol, Attack }
    private State currentState = State.Patrol;

    [Header("Stats")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] GameObject damagerObj;
    private float attackRange = 2.1f;
    private float distanceToPlayer;
    private float moveSpeed = 5f;
    private float attackCooldown = .2f;
    private float attackCounter = 0f;
    private float direction;
    int patrolIndex = 0;
    Transform player;
    private bool disabled = false;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        disabled = GameManager.instance._bosses[2] == 1; //desabilista o inimigos da água se o boss 2 estiver morto
        damagerObj.SetActive(!disabled);
        gameObject.layer = disabled ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        if (isDead || player == null || disabled) return;

        animator.SetBool("Run", currentState == State.Patrol);

        attackCounter += .2f * Time.deltaTime;

        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                if (distanceToPlayer <= attackRange && attackCounter > attackCooldown)
                {
                    ChangeState(State.Attack);
                }
                else
                {
                    HandlePatrol();
                }

                break;

            case State.Attack:
                if (attackCounter > attackCooldown)
                {
                    attackCounter = 0f;
                    rb.velocity = Vector2.zero;
                    animator.SetTrigger("Attack");
                }

                break;
        }

        Flip();
    }

    private void HandlePatrol()
    {
        if (isHurt || currentState == State.Attack) return;

        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[patrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player == null || currentState == State.Attack) return;

        float dir = player.position.x - transform.position.x;
        if (dir != 0)
            direction = Mathf.Sign(dir);

        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    public void FinishAttack() //chamado também na animação Attack
    {
        ChangeState(State.Patrol);
    }

    private void Flip()
    {
        if (isHurt || currentState == State.Attack) return;

        float targetX = currentState == State.Patrol && patrolPoints.Length > 0
            ? patrolPoints[patrolIndex].position.x
            : player.transform.position.x;

        if (transform.position.x < targetX)
            transform.localScale = new Vector2(1, 1);
        else if (transform.position.x > targetX)
            transform.localScale = new Vector2(-1, 1);
    }
}
