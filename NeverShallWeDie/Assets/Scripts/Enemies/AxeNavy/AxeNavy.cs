using System.Collections.Generic;
using UnityEngine;

public class AxeNavy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] private float speed = 2f;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 detectionBoxSize = new Vector2(15f, 1f);
    [SerializeField] private float attackDistance = 2f;

    [SerializeField] private float attackCooldown = 2f; // tempo entre ataques
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    private int currentPatrolIndex = 0;
    private Transform playerTransform;
    private EnemyController controller;
    private bool playerDetected = false;

    private enum State
    {
        Patrolling,
        Chasing,
        Attacking
    }

    private State currentState = State.Patrolling;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    private void Update()
    {
        if (controller._isDead) return;

        controller._animation.SetBool("Attack", currentState == State.Attacking);
        Flip();

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                ChasePlayer();
                break;
            case State.Attacking:
                if (TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.velocity = Vector2.zero;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (controller._isDead || playerTransform == null) return;

        DetectPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackDistance)
        {
            if (!isOnCooldown)
            {
                currentState = State.Attacking;
                isOnCooldown = true;
                cooldownTimer = attackCooldown;
            }
        }
        else if (playerDetected)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Patrolling;
        }

        // Atualizar o cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.fixedDeltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform == null) return;

        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void DetectPlayer()
    {
        playerDetected = Physics2D.OverlapBox(transform.position, detectionBoxSize, 0, playerLayer);
    }

    public void FinishAttack() //chamado pela animação
    {
        if (playerDetected)
            currentState = State.Chasing;
        else
            currentState = State.Patrolling;
    }

    private void Flip()
    {
        if (playerTransform == null) return;

        float targetX = currentState == State.Patrolling
            ? patrolPoints[currentPatrolIndex].position.x
            : playerTransform.position.x;

        if (transform.position.x < targetX)
            transform.localScale = new Vector2(1, 1);
        else if (transform.position.x > targetX)
            transform.localScale = new Vector2(-1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }
}
