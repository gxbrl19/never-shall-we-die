using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOpenWorld : MonoBehaviour
{
    public static ShipOpenWorld instance;

    float _speedMax = 3.5f;
    float _hSpeed;
    float _vSpeed;
    float accelarate = 0.02f;
    float stop = 0.04f;
    [HideInInspector] public bool _canMove;


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
        if (!_canMove) { return; }
        IdleSprite();
        Animation();
    }

    void FixedUpdate()
    {
        if (!_canMove) { return; }
        Accelerate();
    }

    public void Accelerate()
    {
        if (_movement.x > 0 && _hSpeed < _speedMax) { _hSpeed += accelarate; }
        else if (_movement.x < 0 && _hSpeed > -_speedMax) { _hSpeed -= accelarate; }
        else if (_movement.x == 0 && _hSpeed < 0) { _hSpeed += stop; }
        else if (_movement.x == 0 && _hSpeed > 0) { _hSpeed -= stop; }
        if (_movement.x == 0 && _hSpeed == 0) { _hSpeed = 0; }

        if (_movement.y > 0 && _vSpeed < _speedMax) { _vSpeed += accelarate; }
        else if (_movement.y < 0 && _vSpeed > -_speedMax) { _vSpeed -= accelarate; }
        else if (_movement.y == 0 && _vSpeed < 0) { _vSpeed += stop; }
        else if (_movement.y == 0 && _vSpeed > 0) { _vSpeed -= stop; }
        if (_movement.y == 0 && _vSpeed == 0) { _vSpeed = 0; }

        _body.velocity = new Vector2(_hSpeed, _vSpeed);
    }

    public void Animation()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _animation.SetFloat("Horizontal", _hSpeed);
        _animation.SetFloat("Vertical", _vSpeed);

        bool move = _movement.sqrMagnitude != 0; //já retorna true ou false
        _animation.SetBool("Move", move);
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
        //GameManager.instance._shipInitialPosition = transform.position;
    }
}
