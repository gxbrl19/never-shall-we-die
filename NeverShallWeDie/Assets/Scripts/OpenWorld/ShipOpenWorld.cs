using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOpenWorld : MonoBehaviour
{
    public static ShipOpenWorld instance;

    float _accelerationFactor = 3f;
    float _turnFactor = 0.8f;
    float _accelerationInput = 0;
    float _steeringInput = 0;
    float _rotationAngle = 0;
    float _maxSpeed = 4f;

    [HideInInspector] public bool _canMove;
    bool _canAttack = true;

    [SerializeField] Transform _cannonPosition;
    [SerializeField] GameObject _shipCannon;
    [SerializeField] GameObject _cannonEffect;

    [HideInInspector] public bool _submarine;
    [HideInInspector] public Transform _targetSubmarine;
    float _speedSubmarine = 1f;
    float _propulsionForce = 7f;
    float _maxSpeedPropulsion = 7f;

    Transform _transform;
    Rigidbody2D _body;
    Animator _animation;
    Vector2 _movement;
    Collider2D _collider;
    ShipInput _input;

    private void Awake()
    {
        instance = this;
        _transform = GetComponent<Transform>();
        _body = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _input = GetComponent<ShipInput>();
    }

    void Start()
    {
        _canMove = true;

        float _xPosition = GameManager.instance._shipPosX;
        float _yPosition = GameManager.instance._shipPosY;
        transform.position = new Vector3(_xPosition, _yPosition, 0f);
    }

    void Update()
    {
        SubmarineMove();

        if (!_canMove) { return; }
        InputVector();
        Cannon();
    }

    void FixedUpdate()
    {
        if (!_canMove) { return; }
        ApplyEngineForce();
        ApplySteering();
        Propulsion();
        Deceleration();
        _animation.SetFloat("Speed", _body.velocity.magnitude);
    }

    void ApplyEngineForce()
    {
        if (_input.accelerate == 0f || _input.propulsion || !_canMove) { return; }

        Vector2 engineForce = transform.up * _accelerationInput * _accelerationFactor;

        if (_body.velocity.magnitude < _maxSpeed) // Verifica se o corpo já atingiu ou ultrapassou a velocidade máxima na direção da força
        {
            _body.AddForce(engineForce, ForceMode2D.Force);
        }
        else
        {
            float currentSpeed = _body.velocity.magnitude;
            Vector2 brakingForce = -_body.velocity.normalized * (currentSpeed - _maxSpeed) * _accelerationFactor;
            _body.AddForce(brakingForce, ForceMode2D.Force);
        }
    }

    void Deceleration()
    {
        if (_input.accelerate > 0f) { return; }

        if (_body.velocity.magnitude > 0.1f) // Verificar se o Rigidbody está em movimento
        {
            Vector3 decelerationForceVector = -_body.velocity.normalized * _accelerationFactor;
            _body.AddForce(decelerationForceVector, ForceMode2D.Force);
        }
    }

    void ApplySteering()
    {
        _rotationAngle -= _steeringInput * _turnFactor; //muda o angulo de rotação baseado no input
        _body.MoveRotation(_rotationAngle); //aplica a direção de acordo com a rotação do navio
    }

    void InputVector()
    {
        _steeringInput = Input.GetAxisRaw("Horizontal");
        _accelerationInput = _input.accelerate;
    }

    public void SavePos()
    {
        GameManager.instance._shipPosX = transform.position.x;
        GameManager.instance._shipPosY = transform.position.y;
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
        if (_input.accelerate == 0f || !_input.propulsion || !_canMove) { return; }

        Vector2 engineForce = transform.up * _accelerationInput * _propulsionForce;

        if (_body.velocity.magnitude < _maxSpeedPropulsion)
        {
            _body.AddForce(engineForce, ForceMode2D.Force);
        }
    }

    void Cannon()
    {
        if (_canAttack && _input.cannon)
        {
            _canAttack = false;

            Quaternion _rotation = _transform.rotation;
            Instantiate(_shipCannon.gameObject, _cannonPosition.position, _rotation);
            Instantiate(_cannonEffect, _cannonPosition.position, Quaternion.identity);

            Invoke("FinishCannon", 2f);
        }
    }

    public void FinishCannon() //chamado na função Cannon
    {
        _canAttack = true;
    }

    public void StopMove()
    {
        _body.velocity = Vector2.zero;
    }
}
