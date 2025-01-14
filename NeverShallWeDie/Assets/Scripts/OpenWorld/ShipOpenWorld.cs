using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOpenWorld : MonoBehaviour
{
    public static ShipOpenWorld instance;

    float _speedMax = 2.5f;
    float _hSpeed;
    float _vSpeed;
    float accelarate = 0.04f;
    float stop = 0.06f;
    [HideInInspector] public bool _canMove;

    [SerializeField] List<string> _states = new List<string>();
    string _state = "";

    [HideInInspector] public bool _submarine;
    [HideInInspector] public Transform _targetSubmarine;
    float _speedSubmarine = 1f;

    Rigidbody2D _body;
    Animator _animation;
    Vector2 _movement;
    Collider2D _collider;
    ShipInput _input;

    private void Awake()
    {
        instance = this;
        _body = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _input = GetComponent<ShipInput>();
    }

    void Start()
    {
        _canMove = true;

        //verifica se já existe o PlayerPrefs com a posição do Ship
        float _xPosition = PlayerPrefs.HasKey("ShipPositionX") ? PlayerPrefs.GetFloat("ShipPositionX") : 0f;
        float _yPosition = PlayerPrefs.HasKey("ShipPositionY") ? PlayerPrefs.GetFloat("ShipPositionY") : 0f;
        transform.position = new Vector3(_xPosition, _yPosition, 0f);
    }

    void Update()
    {
        SubmarineMove();

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
        if (_submarine) { return; }

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _animation.SetFloat("Horizontal", _hSpeed);
        _animation.SetFloat("Vertical", _vSpeed);

        bool move = _movement.sqrMagnitude != 0; //já retorna true ou false
        _animation.SetBool("Move", move);
    }

    void IdleSprite()
    {
        if (_submarine) { return; }

        if (_movement.x == 0 && _movement.y > 0.01f) { _state = "Up"; }
        else if (_movement.x < -0.01f && _movement.y > 0.01f) { _state = "UpLeft"; }
        else if (_movement.x < -0.01f && _movement.y == 0) { _state = "Left"; }
        else if (_movement.x < -0.01f && _movement.y < -0.01f) { _state = "DownLeft"; }
        else if (_movement.x == 0 && _movement.y < -0.01f) { _state = "Down"; }
        else if (_movement.x > 0.01f && _movement.y < -0.01f) { _state = "DownRight"; }
        else if (_movement.x > 0.01f && _movement.y == 0) { _state = "Right"; }
        else if (_movement.x > 0.01f && _movement.y > 0.01f) { _state = "UpRight"; }

        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i] != _state) { _animation.SetBool(_states[i], false); } else { _animation.SetBool(_states[i], true); }
        }
    }

    public void SavePos()
    {
        PlayerPrefs.SetFloat("ShipPositionX", transform.position.x);
        PlayerPrefs.SetFloat("ShipPositionY", transform.position.y);
    }

    public void SubmarineMove()
    {
        if (_submarine)
        {
            _collider.enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, _targetSubmarine.position, _speedSubmarine * Time.deltaTime);

            if (transform.position == _targetSubmarine.position)
            {
                _animation.SetBool("Submarine", false);
            }
        }
    }

    public void ActiveAnimation()
    {
        _canMove = false;
        _animation.SetBool("Submarine", true);
    }

    public void BeginSubmarine() //chamado na animação
    {
        _submarine = true;
        _input.submarine = false;
    }

    public void FinishSubmarine() //chamado na animação
    {
        _submarine = false;
        _canMove = true;
        _collider.enabled = true;
    }

    void Propulsion()
    {
        if (_input.propulsion)
        {

        }
    }
}
