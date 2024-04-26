using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : EnemyController
{
    public List<Transform> _paths = new List<Transform>();

    public bool _playerTriggered;
    [SerializeField] private GameObject _fire;
    [SerializeField] private Transform _firePoint;

    //private int _direction;
    private int _pathIndex;

    void Update()
    {
        //verifica se o player est� na luz para atacar
        _animation.SetBool("Attack", _playerTriggered);
        Move();
    }

    private void Move()
    {
        if (_isDead || _onHit || _playerTriggered)
            return;
       
        transform.position = Vector2.MoveTowards(transform.position, _paths[_pathIndex].position, 1.3f * Time.deltaTime);
        _animation.SetBool("Move", true);        
        if (Vector2.Distance(transform.position, _paths[_pathIndex].position) < 0.1f)
        {
            _pathIndex = _pathIndex == 0 ? _pathIndex = 1 : _pathIndex = 0;
        }

        Vector2 _dir = _paths[_pathIndex].position - transform.position;            
        if (_dir.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            //_direction = 1;
        }

        if (_dir.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            //_direction = -1;
        }
    }

    public void Fire() //chamado na anima��o de ataque
    {
        Instantiate(_fire, _firePoint.position, Quaternion.identity);
        _playerTriggered = false;        
    }
}
