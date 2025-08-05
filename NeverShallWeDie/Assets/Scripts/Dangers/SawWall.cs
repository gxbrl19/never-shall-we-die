using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawWall : MonoBehaviour
{
    public List<Transform> _paths = new List<Transform>();
    public float _speed;
    private int _pathIndex;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _paths[_pathIndex].position, _speed * Time.deltaTime);

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
    }
}
