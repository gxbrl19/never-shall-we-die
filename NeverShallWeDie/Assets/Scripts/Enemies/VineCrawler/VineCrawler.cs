using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineCrawler : EnemyBase
{
    private enum State { Move }
    private State currentState = State.Move;
    private float speed = 2.5f;
    private int pathIndex;
    public List<Transform> paths = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (isDead) return;

        if (currentState == State.Move)
        {
            transform.position = Vector2.MoveTowards(transform.position, paths[pathIndex].position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, paths[pathIndex].position) < 0.1f)
            {
                if (pathIndex == 0)
                    pathIndex = 1;
                else
                    pathIndex = 0;
            }

            Vector2 dir = paths[pathIndex].position - transform.position;

            /*if (dir.x > 0)
                transform.eulerAngles = new Vector2(0, 0);

            if (dir.x < 0)
                transform.eulerAngles = new Vector2(0, 180);*/
        }
    }
}
