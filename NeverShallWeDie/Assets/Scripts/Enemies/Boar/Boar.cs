using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : EnemyBase
{
    private enum State { Patrol }

    [Header("Comportamento")]
    [SerializeField] Transform[] patrolPoints;

    float patrolSpeed = 6.5f;
    int patrolIndex = 0;
    State currentState;

    private void Start()
    {
        currentState = State.Patrol;
    }

    private void Update()
    {
        if (isDead || isHurt) return;

        switch (currentState)
        {
            case State.Patrol:
                HandlePatrol();
                break;
        }

        Flip();
    }

    private void HandlePatrol()
    {
        if (isHurt) return;

        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[patrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Flip()
    {
        if (isHurt) return;

        float targetX = patrolPoints[patrolIndex].position.x;

        if (transform.position.x < targetX)
            transform.localScale = new Vector2(1, 1);
        else if (transform.position.x > targetX)
            transform.localScale = new Vector2(-1, 1);
    }
}
