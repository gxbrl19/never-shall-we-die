using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDown : MonoBehaviour
{
    public static PlayerTopDown instance;

    public float _speed = 2f;
    public bool _canMove;

    [SerializeField] List<string> _states = new List<string>();    

    Rigidbody2D _body;
    Animator _animation;
    Vector2 _movement;

    private void Awake()
    {
        instance = this;
        _body = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
    }

    void Start()
    {
        _canMove = true;
        transform.position = GameManager.instance._shipInitialPosition;
    }

    void Update()
    {
        IdleSprite();

        if (_canMove)
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            _animation.SetFloat("Horizontal", _movement.x);
            _animation.SetFloat("Vertical", _movement.y);

            if(_movement.sqrMagnitude != 0)//_movement.x != 0 || _movement.y != 0)
            {
                _animation.SetBool("Move", true);
            }
            else
            {
                _animation.SetBool("Move", false);
            }
        }
    }

    void FixedUpdate()
    {
        if(_canMove) 
        {
            _body.velocity = new Vector2(_speed * _movement.x, _speed * _movement.y);
        }
    }

    void IdleSprite()
    {
        string _state = "";

        if (_movement.y > 0) //Up
        {
            _state = "Up";
        }
        else if (_movement.x < 0 && _movement.y == 0) //Left
        {
            _state = "Left";
        }
        else if (_movement.x > 0 && _movement.y == 0) //Right
        {
            _state = "Right";
        }
        else if (_movement.y < 0) //Down
        {
            _state = "Down";
        }

        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i] != _state)
            {
                _animation.SetBool(_states[i], false);
            }
            else
            {
                _animation.SetBool(_states[i], true);
            }
        }     
    }

    public void SavePos()
    {
        GameManager.instance._shipInitialPosition = transform.position;
    }
}
