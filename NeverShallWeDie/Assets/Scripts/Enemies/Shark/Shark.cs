using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : EnemyBase
{
    private enum State { Patrol }

    [Header("Comportamento")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] GameObject damagerObj;

    float patrolSpeed = 5f;
    int patrolIndex = 0;
    State currentState;
    private bool disabled = false;

    private void Start()
    {
        currentState = State.Patrol;

        disabled = GameManager.instance._bosses[2] == 1; //desabilista o inimigos da água se o boss 2 estiver morto
        damagerObj.SetActive(!disabled);
        gameObject.layer = disabled ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Enemy");
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
