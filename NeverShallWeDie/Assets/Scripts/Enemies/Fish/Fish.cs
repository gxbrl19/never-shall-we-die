using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : EnemyController
{
    public List<Transform> _paths = new List<Transform>();

    //private int _direction;
    private int _pathIndex;

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_isDead || _onHit)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _paths[_pathIndex].position, 1.3f * Time.deltaTime);

        if (Vector2.Distance(transform.position, _paths[_pathIndex].position) < 0.1f)
        {
            if (_pathIndex == 0)
            {
                _pathIndex = 1;
            }
            else
            {
                _pathIndex = 0;
            }
        }

        Vector2 _dir = _paths[_pathIndex].position - transform.position;

        if (_dir.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            //_direction = 1;
        }

        if (_dir.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            //_direction = -1;
        }
    }
}
