using UnityEngine;
using FMODUnity;

public class BarrelEnemy : EnemyBase
{
    private enum State { Idle, Roll }
    private State currentState = State.Idle;
    float rollSpeed = 12f;
    float raycastSize = 12f;
    bool detectPlayer = false;
    bool beginRoll = false;
    bool isRolling = false;
    Transform playerPosition;
    [SerializeField] Transform finishPoint;
    [SerializeField] GameObject soundObject;
    [SerializeField] LayerMask playerLayer;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        playerPosition = FindObjectOfType<Player>().transform;
    }

    protected override void Update()
    {
        if (isDead) return;

        DetectPlayer();

        switch (currentState)
        {
            case State.Idle:
                if (detectPlayer)
                    ChangeState(State.Roll);

                break;
            case State.Roll:
                animator.SetTrigger("Roll");

                if (detectPlayer && !beginRoll)
                {
                    isRolling = true;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(finishPoint.position.x, transform.position.y), rollSpeed * Time.deltaTime);
                }

                break;
        }
    }

    public void FinishBeginRoll() //chamado na animação de BeginRoll
    {
        beginRoll = false;
        soundObject.SetActive(true);
    }

    private void ChangeState(State newState)
    {
        if (isDead) return;

        currentState = newState;
    }

    void DetectPlayer()
    {
        if (isDead || isRolling || detectPlayer)
            return;

        Vector2 raycastDirection;
        Vector2 raycastBack;
        raycastDirection = Vector2.right * transform.localScale.x;
        raycastBack = Vector2.left * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastSize, playerLayer);
        RaycastHit2D hitBack = Physics2D.Raycast(transform.position, raycastBack, raycastSize, playerLayer);
        if (hit || hitBack)
        {
            beginRoll = true;
            detectPlayer = true;

            if (hitBack)
                Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer == 8 || other.gameObject.layer == 16) && isRolling) //layer do ground ou katana
        {
            isRolling = false;
            detectPlayer = false;
            isDead = true;
            currentHealth = 0;
            animator.SetTrigger("Dead");
            soundObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 _raycastDirection;
        _raycastDirection = Vector2.right * transform.localScale.x;
        Debug.DrawRay(transform.position, _raycastDirection * raycastSize, Color.red);

        Vector2 _raycastBack;
        _raycastBack = Vector2.left * transform.localScale.x;
        Debug.DrawRay(transform.position, _raycastBack * raycastSize, Color.red);
    }

    void Flip()
    {
        if (isRolling)
            return;

        if (transform.position.x < playerPosition.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (transform.position.x > playerPosition.transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
