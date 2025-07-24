using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Beetboom : EnemyBase
{
    private enum State { Idle, Chase, Explosion }
    private State currentState = State.Idle;

    [Header("Stats")]
    [SerializeField] private LayerMask playerLayer;
    private float attackRange = 2f;
    private float moveSpeed = 6f;
    private float direction;
    private bool playerDetected = false;
    Vector2 detectionBoxSize = new Vector2(8f, 2f);
    Transform player;

    [Header("FMOD Events")]
    [SerializeField] EventReference explodeSFX;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || player == null) return;

        animator.SetBool("Run", currentState == State.Chase);

        DetectPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (playerDetected)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (distanceToPlayer <= attackRange)
                {
                    rb.velocity = Vector2.zero;
                    ChangeState(State.Explosion);
                }
                else
                {
                    MoveTowardsPlayer();
                }

                if (!playerDetected)
                    ChangeState(State.Idle);
                break;

            case State.Explosion:
                animator.SetTrigger("Explosion");
                break;
        }

        FlipTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null || currentState == State.Explosion) return;

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

    private void DetectPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (3f * transform.localScale.x), 0f); //desloca o centro para a direita (2f seria a metade)
        playerDetected = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
    }

    public void PlaySound() //chamado na animação
    {
        RuntimeManager.PlayOneShot(explodeSFX);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Vector2 center = origin + new Vector2(detectionBoxSize.x / (2f * direction), 0f); //desloca o centro para a direita, metade do tamanho
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, detectionBoxSize);
    }
}
